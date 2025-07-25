﻿using Dataverse.ConfigurationMigrationTool.Console.Features.Import;
using Dataverse.ConfigurationMigrationTool.Console.Features.Import.Model;
using Microsoft.Extensions.Logging;

namespace Dataverse.ConfigurationMigrationTool.Console.Services.Filesystem
{
    public class FileReaderDataImportProvider : IImportDataProvider
    {
        private readonly IFileDataReader _dataReader;
        private readonly ILogger<FileReaderDataImportProvider> _logger;

        public FileReaderDataImportProvider(IFileDataReader dataReader,
            ILogger<FileReaderDataImportProvider> logger)
        {
            _dataReader = dataReader;
            _logger = logger;
        }

        public async Task<Entities> ReadFromFile(string filePath)
        {
            _logger.LogInformation("Loading import data from {path}", filePath);
            return await _dataReader.ReadAsync<Entities>(filePath);
        }


        public async Task<ImportSchema> ReadSchemaFromFile(string filePath)
        {
            _logger.LogInformation("Loading schema from {path}", filePath);
            return await _dataReader.ReadAsync<ImportSchema>(filePath);
        }

    }
}
