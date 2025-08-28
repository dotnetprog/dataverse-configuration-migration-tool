using Dataverse.ConfigurationMigrationTool.XrmToolBox.Domain;
using System;
using System.Collections.Generic;

namespace Dataverse.ConfigurationMigrationTool.XrmToolBox.Services
{
    public interface ISolutionService
    {
        IReadOnlyCollection<Solution> GetSolutions();
        IReadOnlyCollection<SolutionEntityComponent> GetEntitiesBySolutionId(Guid solutionId);
    }
}
