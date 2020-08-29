using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Dynamic;
using System.Text.Json;
using MonoGame_Experiments.Components;
using Microsoft.Xna.Framework;
using System.Reflection;

namespace MonoGame_Experiments
{
    public class OgmoTilemapManager
    {

        private static readonly Dictionary<int, Vector2> bitmaskTilePositions = new Dictionary<int, Vector2>()
        {
            { 20,   new Vector2(0, 0) },
            { 68,   new Vector2(1, 0) },
            { 92,   new Vector2(2, 0) },
            { 112,  new Vector2(3, 0) },
            { 28,   new Vector2(4, 0) },
            { 124,  new Vector2(5, 0) },
            { 116,  new Vector2(6, 0) },
            { 80,   new Vector2(7, 0) },
            { 21,   new Vector2(0, 1) },
            { 84,   new Vector2(1, 1) },
            { 87,   new Vector2(2, 1) },
            { 221,  new Vector2(3, 1) },
            { 127,  new Vector2(4, 1) },
            //{ 255,  new Vector2(5, 1) },
            { 241,  new Vector2(6, 1) },
            { 17,   new Vector2(7, 1) },
            { 29,   new Vector2(0, 2) },
            { 117,  new Vector2(1, 2) },
            { 85,   new Vector2(2, 2) },
            { 95,   new Vector2(3, 2) },
            { 247,  new Vector2(4, 2) },
            { 215,  new Vector2(5, 2) },
            { 209,  new Vector2(6, 2) },
            { 1,    new Vector2(7, 2) },
            { 23,   new Vector2(0, 3) },
            { 213,  new Vector2(1, 3) },
            { 81,   new Vector2(2, 3) },
            { 31,   new Vector2(3, 3) },
            { 253,  new Vector2(4, 3) },
            { 125,  new Vector2(5, 3) },
            { 113,  new Vector2(6, 3) },
            { 16,   new Vector2(7, 3) },
            { 5,    new Vector2(0, 4) },
            { 69,   new Vector2(1, 4) },
            { 93,   new Vector2(2, 4) },
            { 119,  new Vector2(3, 4) },
            { 223,  new Vector2(4, 4) },
            { 255,  new Vector2(5, 4) },
            { 245,  new Vector2(6, 4) },
            { 65,   new Vector2(7, 4) },
            { 0,    new Vector2(0, 5) },
            { 4,    new Vector2(1, 5) },
            { 71,   new Vector2(2, 5) },
            { 193,  new Vector2(3, 5) },
            { 7,    new Vector2(4, 5) },
            { 199,  new Vector2(5, 5) },
            { 197,  new Vector2(6, 5) },
            { 64,   new Vector2(7, 5) }
        };

        public static OgmoTilemap LoadLevelData(string tilesetFileName)
        {
            if (tilesetFileName.EndsWith(".json"))
                tilesetFileName = tilesetFileName.Remove(tilesetFileName.Length - 5);

            var assembly = Assembly.GetExecutingAssembly();
            try
            {
                using (Stream stream = assembly.GetManifestResourceStream("MonoGame_Experiments.Ogmo_Project." + tilesetFileName + ".json"))
                using (StreamReader reader = new StreamReader(stream))
                {
                    string result = reader.ReadToEnd();

                    //string result = string.Empty;
                    //result = streamReader.ReadToEnd();
                    OgmoTilemap tilemap = JsonSerializer.Deserialize(result, typeof(OgmoTilemap)) as OgmoTilemap;

                    return tilemap;
                }
            }
            catch
            {
                return default;
            }
        }

        public static int GetTilemapBorders(TilemapLayer layer, int tileID)
        {
            if (layer.dataCoords[tileID][0] == -1) { return -1; }

            int bitwiseIndex = 0;

            bool tl = false;
            bool t = false;
            bool tr = false;
            bool l = false;
            bool r = false;
            bool bl = false;
            bool b = false;
            bool br = false;

            // TODO: Add ability for some tilemap layers to NOT require autotiling. 
            // Ideas: Maybe have a bool property in each tilemap layer like "useAutotile" (ogmo doesn't allow this. Tiled DOES. Noted...).
            // If that idea doesn't work, add "auto_" to the start of the layer or tilemap name, and look for that before parsing the tilemap data.

            // TODO: Extract parts of this into a function with out parameters maybe.
            // Mainly, I need the tiles to act like there IS a tile in a position that is off the tilemap, OR on the next row around.
            // Would be easier to do this if I was using 2D array tile levels, but alas... Speaking of which...
            // TODO: Implement 2D array tilemaps, because they're basically just better overall for everything for the most part.

            CheckTilePosition(layer, tileID, -layer.gridCellsX, out t);
            CheckTilePosition(layer, tileID, -layer.gridCellsX + 1, out tr);
            CheckTilePosition(layer, tileID, +1, out r);
            CheckTilePosition(layer, tileID, +layer.gridCellsX + 1, out br);
            CheckTilePosition(layer, tileID, +layer.gridCellsX, out b);
            CheckTilePosition(layer, tileID, +layer.gridCellsX - 1, out bl);
            CheckTilePosition(layer, tileID, -1, out l);
            CheckTilePosition(layer, tileID, -layer.gridCellsX - 1, out tl);

            if (t)
                bitwiseIndex += 1;
            if (tr && (t && r))
                bitwiseIndex += 2;
            if (r)
                bitwiseIndex += 4;
            if (br && (b && r))
                bitwiseIndex += 8;
            if (b)
                bitwiseIndex += 16;
            if (bl && (b && l))
                bitwiseIndex += 32;
            if (l)
                bitwiseIndex += 64;
            if (tl && (t && l))
                bitwiseIndex += 128;

            return bitwiseIndex;
        }

        private static void CheckTilePosition(TilemapLayer tilemapLayer, int thisTileID, int tileCheckAmount, out bool result)
        {
            result = false;
            int tileCheckID = thisTileID + tileCheckAmount;
            if (MathF.Floor((thisTileID + Math.Sign(tileCheckAmount)) / tilemapLayer.gridCellsX) != MathF.Floor(thisTileID / tilemapLayer.gridCellsX) || 
                thisTileID + tileCheckAmount < 0 || 
                thisTileID + tileCheckAmount > tilemapLayer.dataCoords.Length - 1)
                result = true;
            else
            {
                if (tilemapLayer.dataCoords[tileCheckID][0] != -1)
                    result = true;
            }
        }

        public static Vector2 GetPositionOnBlobTilemap(int bitwiseIndex, int tileWidth = 1, int tileHeight = 1)
        {
            if (bitwiseIndex == -1) { return Vector2.Zero; }
            Vector2 pos = bitmaskTilePositions[bitwiseIndex];
            return new Vector2(pos.X * tileWidth, pos.Y * tileHeight);
        }

        //public static void ParseTilesFromLevelData(TilemapLayer tilemapLayer)
        //{
        //    foreach(List<int> row in tilemapLayer.data2D)
        //    {
        //        foreach(int id in row)
        //        {
        //            Debug.Write(id + " ");
        //        }
        //        Debug.WriteLine("");
        //    }
        //}

    }

    public class OgmoTilemap : IDisposable
    {
        private bool disposedValue;

        public string ogmoVersion { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public int offsetX { get; set; }
        public int offsetY { get; set; }
        public TilemapLayer[] layers { get; set; }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~Tilemap()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }

    public class TilemapLayer : IDisposable
    {
        public string name { get; set; }
        public string _eid { get; set; }
        public int offsetX { get; set; }
        public int offsetY { get; set; }
        public int gridCellWidth { get; set; }
        public int gridCellHeight { get; set; }
        public int gridCellsX { get; set; }
        public int gridCellsY { get; set; }
        public string tileset { get; set; } = null;
        public int[][] dataCoords { get; set; }
        public string[] grid { get; set; }
        public TilemapEntity[] entities { get; set; }
        public int exportMode { get; set; }
        public int arrayMode { get; set; }

        public RenderTarget2D renderTarget2D;

        public class TilemapEntity
        {
            public string name { get; set; }
            public int id { get; set; }
            public string _eid { get; set; }
            public int x { get; set; }
            public int y { get; set; }
            public int originX { get; set; }
            public int originY { get; set; }
            public Vector2[] nodes { get; set; }
            public Dictionary<string, object> values { get; set; }
        }

        private Texture2D tilesetTexture;
        private bool disposedValue;

        //private List<Tile> tiles = new List<Tile>();
        public Texture2D GetTexture2D()
        {
            if (tilesetTexture == null)
            {
                tilesetTexture = ContentHandler.Instance.Load<Texture2D>("Sprites/Tiles/" + tileset);
            }

            return tilesetTexture;
            
        }
        public List<Tile> GetTiles()
        {

            // If we have not loaded the tiles yet...
            List<Tile> list = new List<Tile>();

            for (int i = 0; i < dataCoords.Length; i++)
            {
                int[] tileData = dataCoords[i];
                if (tileData[0] != -1)
                {
                    int xPos = i % gridCellsX;
                    int yPos = (int)MathF.Floor(i / gridCellsX);

                    Vector2 worldPos = new Vector2(xPos * gridCellWidth, yPos * gridCellHeight);

                    Rectangle spriteRectangle = new Rectangle();
                    spriteRectangle.X = tileData[0] * gridCellWidth;
                    spriteRectangle.Y = tileData[1] * gridCellHeight;
                        
                    spriteRectangle.Width = gridCellWidth;
                    spriteRectangle.Height = gridCellHeight;

                    int borderBit = OgmoTilemapManager.GetTilemapBorders(this, i);
                    Vector2 bitPosOnTilemap = OgmoTilemapManager.GetPositionOnBlobTilemap(borderBit, gridCellWidth, gridCellHeight);
                    spriteRectangle.X = (int)bitPosOnTilemap.X;
                    spriteRectangle.Y = (int)bitPosOnTilemap.Y;

                    Tile tile = new Tile(null, GetTexture2D(), worldPos, spriteRectangle, new Vector2(xPos, yPos));
                    tile.TilemapLayer = this;
                    tile.IdOnTilemap = i;

                    list.Add(tile);

                }
            }

            return list;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~TilemapLayer()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
