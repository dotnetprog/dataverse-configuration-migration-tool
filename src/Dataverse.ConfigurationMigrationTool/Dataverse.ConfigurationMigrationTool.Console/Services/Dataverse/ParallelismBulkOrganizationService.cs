using Dataverse.ConfigurationMigrationTool.Console.Common;
using Dataverse.ConfigurationMigrationTool.Console.Services.Dataverse.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.ServiceModel;

namespace Dataverse.ConfigurationMigrationTool.Console.Services.Dataverse
{
    public class OrganizationResponseFaultedResult
    {
        public OrganizationRequest OriginalRequest { get; set; }
        public OrganizationServiceFault Fault { get; set; }
    }
    public class ParallelismBulkOrganizationService : IBulkOrganizationService
    {
        private readonly ServiceClient _serviceClient;
        private readonly ParallelismBulkOrganizationServiceOptions _options;
        private readonly ILogger<ParallelismBulkOrganizationService> logger;

        public ParallelismBulkOrganizationService(ServiceClient serviceClient, IOptions<ParallelismBulkOrganizationServiceOptions> options, ILogger<ParallelismBulkOrganizationService> logger)
        {
            _serviceClient = serviceClient;
            _options = options.Value;
            ThreadPool.SetMinThreads(_options.MaxThreadCount, _options.MaxThreadCount);
            serviceClient.EnableAffinityCookie = false;
            this.logger = logger;
        }

        public async Task<IEnumerable<OrganizationResponseFaultedResult>> ExecuteBulk(IEnumerable<OrganizationRequest> request, IEnumerable<string> faultToSkips = null)
        {
            var result = new ConcurrentBag<OrganizationResponseFaultedResult>();
            logger.LogInformation("{count} request(s) to execute.", request.Count());

            var batches = request.Batch(_options.BatchSize);
            logger.LogInformation("{requestioncount} request(s) that is packaged into {batchcount} batch(es) will be executed.", request.Count(), batches.Count());
            var parallelOptions = new ParallelOptions()
            {
                MaxDegreeOfParallelism = _options.MaxDegreeOfParallism
            };
            await Parallel.ForEachAsync(
               source: batches,
               parallelOptions: parallelOptions,
               async (batch, token) =>
               {
                   logger.LogInformation("Processing a batch of {count} requests, current threads({countThreads})", batch.Count(), Process.GetCurrentProcess().Threads.Count);
                   var scopedService = _serviceClient.Clone();
                   scopedService.EnableAffinityCookie = false;
                   var requests = new OrganizationRequestCollection();
                   requests.AddRange(batch);
                   var request = new ExecuteMultipleRequest
                   {
                       Requests = requests,
                       Settings = new ExecuteMultipleSettings
                       {
                           ContinueOnError = true,
                           ReturnResponses = true,
                       }
                   };
                   logger.LogInformation("Starting a batch of {count} requests", requests.Count);
                   var response = (await scopedService.ExecuteAsync(request)) as ExecuteMultipleResponse;
                   var faultedResponses = response.Responses
                      .Where(r => r.Fault != null && (faultToSkips?.All(f => !r.Fault.Message.Contains(f)) ?? true))
                      .Select(fr => new OrganizationResponseFaultedResult { Fault = fr.Fault, OriginalRequest = requests[fr.RequestIndex] }).ToArray();
                   if (faultedResponses.Any())
                   {

                       logger.LogInformation("A Batch finished with {count} failures", faultedResponses.Length);
                       foreach (var faultedResponse in faultedResponses)
                       {
                           result.Add(faultedResponse);
                       }
                   }
               });

            return result;

        }

        public async Task<Result<UpsertResponse, OrganizationResponseFaultedResult>> Upsert(UpsertRequest request)
        {
            var setStateRquest = new UpdateRequest();
            if (request.Target.Attributes.ContainsKey("statecode") ||
            request.Target.Attributes.ContainsKey("statuscode"))
            {
                setStateRquest.Target = new Entity();
                if (request.Target.Attributes.ContainsKey("statecode"))
                {
                    setStateRquest.Target.Attributes["statecode"] = request.Target.Attributes["statecode"];
                }
                if (request.Target.Attributes.ContainsKey("statuscode"))
                {
                    setStateRquest.Target.Attributes["statuscode"] = request.Target.Attributes["statuscode"];
                }

                request.Target.Attributes.Remove("statecode");
                request.Target.Attributes.Remove("statuscode");
            }
            try
            {
                var response = (await _serviceClient.ExecuteAsync(request)) as UpsertResponse;
                if (setStateRquest.Target != null)
                {
                    setStateRquest.Target.LogicalName = response.Target.LogicalName;
                    setStateRquest.Target.Id = response.Target.Id;
                    await _serviceClient.ExecuteAsync(setStateRquest);
                }
                return new Result<UpsertResponse, OrganizationResponseFaultedResult>(response);
            }
            catch (FaultException<OrganizationServiceFault> faultEx)
            {
                return new Result<UpsertResponse, OrganizationResponseFaultedResult>(new OrganizationResponseFaultedResult { Fault = faultEx.Detail, OriginalRequest = request });
            }

        }

        public async Task<IEnumerable<OrganizationResponseFaultedResult>> UpsertBulk(IEnumerable<UpsertRequest> requests)
        {
            var result = new ConcurrentBag<OrganizationResponseFaultedResult>();
            var requestsCount = requests.Count();
            int processCount = 0;
            logger.LogInformation("{count} rows(s) to execute.", requestsCount);
            var parallelOptions = new ParallelOptions()
            {
                MaxDegreeOfParallelism = _options.MaxDegreeOfParallism < _serviceClient.RecommendedDegreesOfParallelism ?
                _options.MaxDegreeOfParallism : _serviceClient.RecommendedDegreesOfParallelism
            };
            var batches = requests.Batch(_options.BatchSize);
            await Parallel.ForEachAsync(source: batches, parallelOptions: parallelOptions,
                async (batch, token) =>
                {
                    var scopedService = _serviceClient.Clone();

                    scopedService.EnableAffinityCookie = false;
                    foreach (var request in batch)
                    {
                        var setStateRquest = new UpdateRequest();
                        if (request.Target.Attributes.ContainsKey("statecode") ||
                        request.Target.Attributes.ContainsKey("statuscode"))
                        {
                            setStateRquest.Target = new Entity();
                            if (request.Target.Attributes.ContainsKey("statecode"))
                            {
                                setStateRquest.Target.Attributes["statecode"] = request.Target.Attributes["statecode"];
                            }
                            if (request.Target.Attributes.ContainsKey("statuscode"))
                            {
                                setStateRquest.Target.Attributes["statuscode"] = request.Target.Attributes["statuscode"];
                            }

                            request.Target.Attributes.Remove("statecode");
                            request.Target.Attributes.Remove("statuscode");
                        }
                        try
                        {
                            var response = (await scopedService.ExecuteAsync(request)) as UpsertResponse;
                            if (setStateRquest.Target != null)
                            {
                                setStateRquest.Target.LogicalName = response.Target.LogicalName;
                                setStateRquest.Target.Id = response.Target.Id;
                                await scopedService.ExecuteAsync(setStateRquest);
                            }

                        }
                        catch (FaultException<OrganizationServiceFault> faultEx)
                        {
                            result.Add(new OrganizationResponseFaultedResult { Fault = faultEx.Detail, OriginalRequest = request });
                        }
                        finally
                        {
                            Interlocked.Increment(ref processCount);
                        }
                    }


                    logger.LogInformation("Proccessed: {proccessedCount}/{requestCount}, Threads({threadcount})", processCount, requestsCount, Process.GetCurrentProcess().Threads.Count);


                });
            return result;
        }
    }
}
