using Dataverse.ConfigurationMigrationTool.XrmToolBox.Domain.Abstraction;
using System.IO;
using System.Xml.Serialization;

namespace Dataverse.ConfigurationMigrationTool.XrmToolBox.Services
{
    public class XmlFileService : IFileService
    {
        public void WriteToFile<T>(string filePath, T data)
        {
            var x = new XmlSerializer(typeof(T));
            using (var writer = new StreamWriter(filePath))
            {
                x.Serialize(writer, data);
            }
        }
    }
}
