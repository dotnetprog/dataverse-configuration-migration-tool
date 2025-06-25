using Dataverse.ConfigurationMigrationTool.Console.Services.Dataverse.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using System.Collections.Concurrent;

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

        public async Task<IEnumerable<OrganizationResponseFaultedResult>> ExecuteBulk(IEnumerable<OrganizationRequest> request)
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
                   if (response.IsFaulted)
                   {
                       var faultedResponses = response.Responses
                       .Where(r => r.Fault != null)
                       .Select(fr => new OrganizationResponseFaultedResult { Fault = fr.Fault, OriginalRequest = requests[fr.RequestIndex] }).ToArray();
                       logger.LogInformation("A Batch finished with {count} failures", faultedResponses.Length);
                       foreach (var faultedResponse in faultedResponses)
                       {
                           result.Add(faultedResponse);
                       }
                   }
               });

            return result;

        }


    }
}
