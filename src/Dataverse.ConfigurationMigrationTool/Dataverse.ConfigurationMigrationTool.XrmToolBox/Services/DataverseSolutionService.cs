using Dataverse.ConfigurationMigrationTool.XrmToolBox.Common;
using Dataverse.ConfigurationMigrationTool.XrmToolBox.Domain;
using Dataverse.ConfigurationMigrationTool.XrmToolBox.Mappers;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dataverse.ConfigurationMigrationTool.XrmToolBox.Services
{
    public class DataverseSolutionService : ISolutionService
    {
        private readonly IOrganizationService _organizationService;
        private static readonly IMapper<Entity, Solution> _mapper = new EntityToSolutionMapper();
        private static readonly IMapper<Entity, SolutionEntityComponent> _entityComponentMapper = new EntityToSolutionComponentMapper();

        public DataverseSolutionService(IOrganizationService organizationService)
        {
            _organizationService = organizationService;
        }

        public IReadOnlyCollection<Solution> GetSolutions()
        {
            var query = new QueryExpression("solution")
            {
                ColumnSet = new ColumnSet("friendlyname", "uniquename", "version", "ismanaged")
            };
            query.AddOrder("friendlyname", OrderType.Ascending);
            var result = _organizationService.RetrieveMultiple(query);
            return result.Entities.Select(_mapper.Map).ToArray();
        }

        public IReadOnlyCollection<SolutionEntityComponent> GetEntitiesBySolutionId(Guid solutionId)
        {
            var query = new QueryExpression("solutioncomponent")
            {
                ColumnSet = new ColumnSet("solutionid", "componenttype", "objectid"),
                Criteria = new FilterExpression
                {
                    Conditions =
                    {
                        new ConditionExpression("solutionid", ConditionOperator.Equal, solutionId),
                        new ConditionExpression("componenttype", ConditionOperator.Equal, 1) // 1 is for Entity
                    }
                },
                LinkEntities =
                {
                    new LinkEntity
                    {
                        LinkFromEntityName = "solutioncomponent",
                        LinkFromAttributeName = "objectid",
                        LinkToEntityName = "entity",
                        LinkToAttributeName = "entityid",
                        Columns = new ColumnSet("logicalname","originallocalizedname","name"),
                        EntityAlias = "entity",
                        JoinOperator = JoinOperator.Inner
                    }
                }
            };

            var result = _organizationService.RetrieveMultiple(query);
            return result.Entities.Select(e => _entityComponentMapper.Map(e.GetAliasedEntity("entity"))).ToArray();
        }
    }
}
