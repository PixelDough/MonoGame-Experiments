using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace OgmoMapPipeline
{

    public class Tilemap
    {
        [JsonProperty("ogmoVersion")]
        public string ogmoVersion;
        public int width;
        public int height;
        public int offsetX;
        public int offsetY;
        public List<TilemapLayer> layers;
    }

    public class TilemapLayer
    {
        public string name;
        public string _eid;
        public int offsetX;
        public int offsetY;
        public int gridCellWidth;
        public int gridCellHeight;
        public int gridCellsX;
        public int gridCellsY;
        public string tileset;
        public List<List<int>> dataCoords;
        public List<string> grid;
        public TilemapEntity[] entities;
        public int exportMode;
        public int arrayMode;

        public class TilemapEntity
        {
            public string name;
            public int id;
            public string _eid;
            public int x;
            public int y;
            public int originX;
            public int originY;
            public List<Vector2> nodes;
            public Dictionary<string, object> values;
        }
    }
}
