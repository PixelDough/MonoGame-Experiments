using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Dynamic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using MonoGame_Experiments.Components;
using Microsoft.Xna.Framework;
using System.Linq;

namespace MonoGame_Experiments
{
    public class OgmoTilemapManager
    {
        
        public static Tilemap LoadLevelData(string filename)
        {
            if (!File.Exists(filename))
            {
                throw new Exception("That level data does not exist.");
            }

            using (StreamReader streamReader = new StreamReader(filename))
            {
                string result = string.Empty;
                result = streamReader.ReadToEnd();
                Tilemap tilemap = JsonSerializer.Deserialize(result, typeof(Tilemap)) as Tilemap;
                //Dictionary<string, object> dictionary = JsonSerializer.Deserialize< Dictionary<string, object>>(result);
                
                //foreach(TilemapLayer layer in tilemap.layers)
                //{
                //    foreach (var entity in layer.entities ?? Enumerable.Empty<TilemapLayer.TilemapEntity>())
                //    {
                //        foreach (var val in entity.values ?? Enumerable.Empty<KeyValuePair<string, object>>())
                //        {
                //            Debug.WriteLine(val.Key + " : " + val.Value);
                //        }
                //    }
                //}
                
                //Debug.WriteLine(tilemap.layers[0].data2D[0][0]);
                //Debug.WriteLine(tilemap.layers[0].tileset);
                return tilemap;
            }
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

    public class Tilemap : DynamicObject
    {
        public string ogmoVersion { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public int offsetX { get; set; }
        public int offsetY { get; set; }
        public TilemapLayer[] layers { get; set; }
    }

    public class TilemapLayer : DynamicObject
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
        private List<Tile> tiles;
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
            List<Tile> tiles = new List<Tile>();

            // If we have not loaded the tiles yet...
            if (tiles == null || tiles.Count <= 0)
            {
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

                        Tile tile = new Tile(GetTexture2D(), worldPos, spriteRectangle);
                        tile.TilemapLayer = this;
                        tile.IdOnTilemap = i;
                        tiles.Add(tile);
                    }
                }

                this.tiles = tiles;
            }
            else
            {
                tiles = this.tiles;
            }

            return tiles;
        }
    }
}
