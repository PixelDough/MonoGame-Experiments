using Microsoft.Xna.Framework.Content.Pipeline;
using Newtonsoft.Json;
using System.IO;
using TImport = System.String;

namespace OgmoMapPipeline
{
    [ContentImporter(".json", DisplayName = "Ogmo Map Importer", DefaultProcessor = "OgmoMapProcessor")]
    public class OgmoMapImporter : ContentImporter<Tilemap>
    {
        public override Tilemap Import(string filename, ContentImporterContext context)
        {
            context.Logger.LogMessage("Importing JSON map: {0}", filename);

            using (var file = File.OpenText(filename))
            {
                var serializer = new JsonSerializer();
                var serializedMap = (Tilemap)serializer.Deserialize(file, typeof(Tilemap));

                return serializedMap;
            }
        }
    }
}
