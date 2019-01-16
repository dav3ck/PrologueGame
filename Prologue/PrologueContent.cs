using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace Prologue
{
    class PrologueContent
    {
        public Texture2D Tile1 { get; set; }
        public Texture2D Tile2 { get; set; }
        public Texture2D imgPlayer { get; set; } 
        public SpriteFont Arial20 { get; set; }

        public PrologueContent(ContentManager Content)
        {
            Tile1 = Content.Load<Texture2D>("Tile1");
            Tile2 = Content.Load<Texture2D>("Tile2");
            imgPlayer = Content.Load<Texture2D>("PlayerPlaceHolder");

            Arial20 = Content.Load<SpriteFont>("Arial20");
        }
    }
}
