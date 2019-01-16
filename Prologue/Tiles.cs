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
    class Tiles
    {
        public int X { get; set; }
        public int Y { get; set; }
        public float Xcord { get; set; }
        public float Ycord { get; set; }
        public int ID { get; set; }
        public bool Solid { get; set; }

        public Texture2D imgtile { get; set; }
        private SpriteBatch spriteBatch;

        public Tiles(int X, int Y, int ID, bool Solid, float Xcord, float Ycord, Texture2D tileimage, SpriteBatch spriteBatch)
        {
            this.X = X;
            this.Y = Y;
            this.ID = ID;
            this.Solid = Solid;
            this.Xcord = Xcord;
            this.Ycord = Ycord;
            this.imgtile = tileimage;
            this.spriteBatch = spriteBatch;
        }

        public void Draw(float GridSize)
        {
            spriteBatch.Draw(imgtile,new Rectangle((int)(this.Xcord - Screen.CameraX),(int)(this.Ycord - Screen.CameraY),(int)GridSize,(int)GridSize), Color.White);
        }
    }
}
