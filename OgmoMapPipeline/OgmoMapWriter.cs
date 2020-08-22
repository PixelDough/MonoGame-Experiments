using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using System;
using System.Collections.Generic;
using System.Text;

namespace OgmoMapPipeline
{
    [ContentTypeWriter]
    class OgmoMapWriter : ContentTypeWriter<OgmoMapProcessorResult>
    {
        protected override void Write(ContentWriter output, OgmoMapProcessorResult value)
        {
            output.Write(value.Map.width);
            output.Write(value.Map.height);

            output.Write(value.Map.layers.Count);

            foreach(var layer in value.Map.layers)
            {
                output.Write(layer.name);
                output.Write(layer.dataCoords.Count);

                foreach (var tile in layer.dataCoords)
                {
                    output.Write(tile.ToString());
                }

                
            }


        }

        public override string GetRuntimeType(TargetPlatform targetPlatform)
        {
            return typeof(Tilemap).AssemblyQualifiedName;
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return "PixelDough.Maps.MapReader, PixelDough";
        }
    }
}
