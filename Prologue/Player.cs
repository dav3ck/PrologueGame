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
    class Player
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public float width { get; set; }
        public float height { get; set; }
        public bool Frozen { get; set; }
        public int scale { get; set; }

        public float Vertical_Momentum { get; set; }
        public float Horizontal_Momentum { get; set; }
        public List<Tiles> Tilelist { get; set; }

        public float HitboxWidthMargin { get; set; }
        public float HitboxHeight { get; set; }
        public float HitboxWidth { get; set; }
        public float HitboxHeight2 { get; set; }
        public Rectangle Hitbox;

        private Texture2D imgPlayer { get; set; }
        private SpriteBatch FrontSpriteBatch;
        private PrologueContent prologueContent { get; set; }

        public Player(float X, float Y, SpriteBatch FrontSpriteBatch, PrologueContent prologueContent, List<Tiles> Tilelist)
        {
            this.X = X;
            this.Y = Y;
            this.Frozen = false;
            imgPlayer = prologueContent.imgPlayer;
            this.width = imgPlayer.Width;
            this.height = imgPlayer.Height;
            this.FrontSpriteBatch = FrontSpriteBatch;
            this.scale = 5;
            this.Vertical_Momentum = 0;
            this.Horizontal_Momentum = 0;
            this.Tilelist = Tilelist;
            this.prologueContent = prologueContent;

            this.Width = this.width * this.scale;
            this.Height = this.height * this.scale;

            this.HitboxWidthMargin = (this.Width - Screen.GridSize) / 2 + 10 ;
            this.HitboxHeight = Screen.GridSize + 20;

            this.HitboxHeight2 = Screen.GridSize - 30;
            this.HitboxWidth = this.Width - (this.HitboxWidthMargin * 2);

        }


        public void Draw()
        {
            
            FrontSpriteBatch.Draw(imgPlayer, new Rectangle((int)(this.X - Screen.CameraX), (int)(this.Y - Screen.CameraY), (int)this.Width, (int)this.Height), Color.White);
           // FrontSpriteBatch.Draw(prologueContent.Tile1, this.Hitbox, Color.Yellow);
        }



        public void HorizontalMov(string Direction)
        {
            //If player Input is given it wil test for the direct pressed if the Player is actually allowed to mvoe that way. 
            // Horizontal Movement & vertical Movement are split so when you can no longer walk up, but u stil have the Left key pressed it wil continue wall hugging to the left

            if (Frozen == true) { return; }
            bool Allowmovement = true;

            if (Direction == "Left")
            {
                Horizontal_Momentum = -3;
                Allowmovement = AllowMovement(Tuple.Create(this.X + Horizontal_Momentum + this.HitboxWidthMargin, this.Y + HitboxHeight), Tuple.Create(this.X + Horizontal_Momentum + this.HitboxWidthMargin, this.Y + this.Height));
            }
            else if (Direction == "Right")
            {
                Horizontal_Momentum = 3;
                Allowmovement = AllowMovement(Tuple.Create(this.X + Horizontal_Momentum + this.Width - this.HitboxWidthMargin, this.Y + HitboxHeight), Tuple.Create(this.X + Horizontal_Momentum + this.Width - this.HitboxWidthMargin, this.Y + this.Height));
            }
            if (Allowmovement == false) { Horizontal_Momentum = 0; }
        }

        public void VerticalMov(string Direction)
        {
            if (Frozen == true) { return; }
            bool Allowmovement = true;

            if (Direction == "Up")
            {
                Vertical_Momentum = -2;
                Allowmovement = AllowMovement(Tuple.Create(this.X + this.HitboxWidthMargin, this.Y + Vertical_Momentum + HitboxHeight), Tuple.Create(this.X + this.Width - this.HitboxWidthMargin, this.Y + Vertical_Momentum + HitboxHeight));
            }
            else if (Direction == "Down")
            {
                Vertical_Momentum = 2;
                Allowmovement = AllowMovement(Tuple.Create(this.X + this.HitboxWidthMargin, this.Y + Vertical_Momentum + this.Height), Tuple.Create(this.X + this.Width - this.HitboxWidthMargin, this.Y + Vertical_Momentum + this.Height));
            }

            if (Allowmovement == false) { Vertical_Momentum = 0; }
        }

        public void Update()
        {
            Hitbox = new Rectangle((int)(this.X + this.HitboxWidthMargin), (int)(this.Y + this.Height - this.HitboxHeight2), (int)this.HitboxWidth, (int)this.HitboxHeight2);

            Screen.CameraMovement(this.X, this.Y);
            this.X += Horizontal_Momentum;
            Screen.CameraMovement(this.X, this.Y);
            this.Y += Vertical_Momentum;
            

            Horizontal_Momentum = 0;
            Vertical_Momentum = 0;
        }

        public bool AllowMovement(Tuple<float,float> cord1, Tuple<float, float> cord2)
        {
            Tuple<int, int> GridCord1 = Screen.GridCords(cord1.Item1, cord1.Item2);
            Tuple<int, int> GridCord2 = Screen.GridCords(cord2.Item1, cord2.Item2);

            Rectangle RecCord1 = new Rectangle((int)cord1.Item1, (int)cord1.Item2,1,1);
            Rectangle RecCord2 = new Rectangle((int)cord2.Item1, (int)cord2.Item2,1,1);

            foreach (var tile in Tilelist)
            {
                if (tile.X == GridCord1.Item1 && tile.Y == GridCord1.Item2 && tile.Solid == true)
                {
                    return false;
                    
                }
                else if (tile.X == GridCord2.Item1 && tile.Y == GridCord2.Item2 && tile.Solid == true)
                {
                    return false;
                }
            }

            foreach (var _Object in Objects.ObjectList)
            {
                if (_Object.Xtile == GridCord1.Item1 && _Object.Ytile == GridCord1.Item2 && _Object.Solid == true)
                {
                    return false;

                }
                else if (_Object.Xtile == GridCord2.Item1 && _Object.Ytile == GridCord2.Item2 && _Object.Solid == true)
                {
                    return false;
                }
            }

            foreach (var NPC in NPC.NPClist)
            {
                if (RecCord1.Intersects(NPC.Hitbox))
                {
                    return false;
                }
                if (RecCord2.Intersects(NPC.Hitbox))
                {
                    return false;
                }
            }  
            return true; 
        }

        /*public bool AllowMovementNPC(Tuple<float,float> cord1, Tuple<float,float> cord2)
        {
            return true;
        } */

        public Tuple<float, float> GetPlayerPosition()
        {
            return Tuple.Create(X, Y);
        }

    }
}
