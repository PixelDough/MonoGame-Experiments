using Microsoft.Xna.Framework.Content.Pipeline;
using System;
using System.Collections.Generic;
using System.Text;

namespace OgmoMapPipeline
{
    class OgmoMapProcessorResult
    {
        public Tilemap Map;
        public ContentBuildLogger Logger;

        public OgmoMapProcessorResult(Tilemap map, ContentBuildLogger logger)
        {
            Map = map;
            Logger = logger;
        }
    }
}
