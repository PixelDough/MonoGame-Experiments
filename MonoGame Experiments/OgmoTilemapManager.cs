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
        public string tileset { get; set; }
        //[JsonPropertyName("tileset")]
        //private string _tilesetTextureName { set { tilesetTexture = ContentHandler.Instance.Load<Texture2D>("Sprites/Tiles/" + value); } }
        public List<List<int>> data2D { get; set; }
        public int[][][] dataCoords2D { get; set; }
        public int exportMode { get; set; }
        public int arrayMode { get; set; }

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
                int yPos = 0;
                foreach (var row in dataCoords2D)
                {
                    int xPos = 0;
                    foreach (var coord in row)
                    {
                        if (coord[0] != -1)
                        {
                            Vector2 worldPos = new Vector2(xPos * gridCellWidth, yPos * gridCellHeight);

                            Rectangle spriteRectangle = new Rectangle();
                            spriteRectangle.X = coord[0] * gridCellWidth;
                            spriteRectangle.Y = coord[1] * gridCellHeight;
                            spriteRectangle.Width = gridCellWidth;
                            spriteRectangle.Height = gridCellHeight;

                            Tile tile = new Tile(GetTexture2D(), worldPos, spriteRectangle);
                            tiles.Add(tile);
                        }
                        xPos++;
                    }
                    yPos++;
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
