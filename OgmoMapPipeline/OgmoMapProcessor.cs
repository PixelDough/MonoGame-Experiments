using Microsoft.Xna.Framework.Content.Pipeline;

using TInput = System.String;
using TOutput = System.String;

namespace OgmoMapPipeline
{
    [ContentProcessor(DisplayName = "Ogmo Map Processor")]
    class OgmoMapProcessor : ContentProcessor<Tilemap, OgmoMapProcessorResult>
    {
        public override OgmoMapProcessorResult Process(Tilemap input, ContentProcessorContext context)
        {
            return new OgmoMapProcessorResult(input, context.Logger);
        }
    }
}
