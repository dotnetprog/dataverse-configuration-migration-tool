using System.Text;
using System.Xml.Serialization;

namespace Dataverse.ConfigurationMigrationTool.Console.Services.Filesystem
{
    public class XmlFileDataReader : IFileDataReader
    {
        public async Task<T> ReadAsync<T>(string path)
        {
            var xml = await File.ReadAllTextAsync(path, Encoding.UTF8);
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (StringReader reader = new StringReader(xml))
            {
                var data = (T)serializer.Deserialize(reader);
                return data;
            }
        }
    }
}
