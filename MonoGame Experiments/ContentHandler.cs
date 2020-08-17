using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame_Experiments
{
    public class ContentHandler : ContentManager
    {
        public static ContentHandler Instance;

        public ContentHandler(IServiceProvider serviceProvider, string rootDirectory) : base(serviceProvider, rootDirectory) 
        {
            if (Instance == null)
                Instance = this;
        }

        public static Texture2D LoadTexture2D(string textureDirectory)
        {
            return Instance.Load<Texture2D>(textureDirectory);
        }
    }
}
