using Cocona;
using Dataverse.ConfigurationMigrationTool.Console.Features.Import.Model;
using Microsoft.Extensions.Logging;

namespace Dataverse.ConfigurationMigrationTool.Console.Features.Import.Commands;

public class ImportCommands
{
    private readonly ILogger<ImportCommands> _logger;
    private readonly IImportDataProvider _importDataProvider;

    public ImportCommands(ILogger<ImportCommands> logger,
        IImportDataProvider importDataProvider)
    {
        _logger = logger;
        _importDataProvider = importDataProvider;
    }

    [Command("import")]
    public async Task Import([Option("schema")] string schemafilepath, [Option("data")] string datafilepath)
    {

        var ImportQueue = new Queue<ImportDataTask>();

        var schema = await _importDataProvider.ReadSchemaFromFile(schemafilepath);
        var importdata = await _importDataProvider.ReadFromFile(datafilepath);



    }

}
