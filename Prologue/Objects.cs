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
    class Objects
    {
        public int Xtile { get; set; }
        public int Ytile { get; set; }

        public float Xcord { get; set; }
        public float Ycord { get; set; }

        public bool Solid { get; set; }

        public string Information { get; set; }

        public SpriteBatch FrontSpriteBatch;
        public PrologueContent prologueContent { get; set; }
        public Texture2D Objectimg { get; set; }

        public bool CanInteract { get; set; }

        public static List<Objects> ObjectList = new List<Objects>();

        //------------------------------------------------------------

        public Objects(int _Xtile, int _Ytile, string _Information, SpriteBatch FrontSpriteBatch, PrologueContent prologueContent)
        {
            this.Xtile = _Xtile;
            this.Ytile = _Ytile;

            this.Information = _Information;

            this.prologueContent = prologueContent;
            this.FrontSpriteBatch = FrontSpriteBatch;

            this.Objectimg = prologueContent.Tile1;


            Tuple<float, float> _Screencords = Screen.ScreenCords(this.Xtile, this.Ytile);
            this.Xcord = _Screencords.Item1;
            this.Ycord = _Screencords.Item2;
            this.Solid = true;

            ObjectList.Add(this);
        }


        public void Update(Rectangle PlayerHitbox)
        {

            //Collision with player is tested here (whether or not the object is interactable)
            //It Makes a new Rectangle slightly out of the middle of the Hitbox of the Object
            //Then checks whether or not this hitbox collides with the Player Hitbox

            CanInteract = false;

            Rectangle _HitboxObject = new Rectangle((int)this.Xcord, (int)this.Ycord, (int)Screen.GridSize, (int)Screen.GridSize);

            Rectangle _Side1 = new Rectangle((int)this.Xcord - 20, (int)this.Ycord + _HitboxObject.Height / 2, 3, 3);
            Rectangle _Side2 = new Rectangle((int)this.Xcord + _HitboxObject.Width / 2, (int)this.Ycord - 20, 3, 3);
            Rectangle _Side3 = new Rectangle((int)this.Xcord + 20 + _HitboxObject.Width, (int)this.Ycord + _HitboxObject.Height / 2, 3, 3);
            Rectangle _Side4 = new Rectangle((int)this.Xcord + _HitboxObject.Width / 2, (int)this.Ycord + 20 + _HitboxObject.Height, 3, 3);

            if(_Side1.Intersects(PlayerHitbox) || _Side2.Intersects(PlayerHitbox) || _Side3.Intersects(PlayerHitbox) || _Side4.Intersects(PlayerHitbox))
            {
                CanInteract = true;
            }

            //Temperary Visualisation of this

            if (CanInteract == true)
            {
                this.Objectimg = prologueContent.Tile2;
            }
            else
            {
                this.Objectimg = prologueContent.Tile1;
            }
        } 

        public void Draw()
        {
            FrontSpriteBatch.Draw(Objectimg, new Rectangle((int)(this.Xcord - Screen.CameraX), (int)(this.Ycord - Screen.CameraY), (int)Screen.GridSize, (int)Screen.GridSize), Color.White);
        }

    }

    //Eventually certain Objects can do Certain things, So I made the main object class with properties all objects share, and then wil have inheritand classes for the special features, Like a computer that can load

    class NormalObject : Objects
    {
        public NormalObject(int _Xtile, int _Ytile, string _Text, SpriteBatch FrontSpriteBatch, PrologueContent prologueContent) : base(_Xtile, _Ytile, _Text, FrontSpriteBatch, prologueContent)
        {
        }

        public void Interact()
        {
            InformationTextBox Textbox2 = new InformationTextBox(new Vector2(100, 100), this.Information, 200, 200, this.FrontSpriteBatch, this.prologueContent);
        }

    }
}
