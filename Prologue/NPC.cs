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
    class NPC
    {
        public float X { get; set; }
        public float Y { get; set; }

        public float X_towalk { get; set; }
        public float Y_towalk { get; set; }

        public float Xnormalized { get; set; }
        public float Ynormalized { get; set; }

        public int Xtile { get; set; }
        public int Ytile { get; set; }

        public float Width { get; set; }
        public float Height { get; set; }
        public float width { get; set; }
        public float height { get; set; }

        public float HitboxHeight { get; set; }
        public float HitboxWidth { get; set; }
        public float HitboxWidthMargin { get; set; }

        public List<Tiles> Tilelist { get; set; }

        public int scale { get; set; }
        public Rectangle Hitbox;

        public float Vertical_Momentum { get; set; }
        public float Horizontal_Momentum { get; set; }
        public bool Moving { get; set; }
        public Tuple<float, float> Destination { get; set; }


        public Texture2D NPCimg { get; set; }
        private SpriteBatch FrontSpriteBatch;
        PrologueContent prologueContent { get; set; }

        static public List<NPC> NPClist = new List<NPC>();

        private List<PathData> Path = new List<PathData>();
        public int TilesWalked { get; set; }

        public NPC(int Xtile, int Ytile, SpriteBatch FrontSpriteBatch, PrologueContent prologueContent, List<Tiles> Tilelist)
        {
            this.Xtile = Xtile;
            this.Ytile = Ytile;

            NPCimg = prologueContent.imgPlayer;
            this.width = NPCimg.Width;
            this.height = NPCimg.Height;
            this.FrontSpriteBatch = FrontSpriteBatch;
            this.scale = 5;
            this.Vertical_Momentum = 0;
            this.Horizontal_Momentum = 0;
            this.Moving = false;
            this.Tilelist = Tilelist;
            this.prologueContent = prologueContent;

            this.Width = this.width * this.scale;
            this.Height = this.height * this.scale;

            this.HitboxWidthMargin = (this.Width - Screen.GridSize) / 2;

            this.HitboxHeight = Screen.GridSize - 30;
            this.HitboxWidth = this.Width - (this.HitboxWidthMargin * 2);
            Hitbox = new Rectangle((int)(this.X + this.HitboxWidthMargin), (int)(this.Y + this.Height - this.HitboxHeight), (int)this.HitboxWidth, (int)this.HitboxHeight);

            NPClist.Add(this);

            this.Xnormalized = Xtile * Screen.GridSize;
            this.Ynormalized = Ytile * Screen.GridSize + Screen.GridSize - 1;

            this.X = this.Xnormalized + Screen.GridSize / 2 - this.Width / 2 ; 
            this.Y = Ytile * Screen.GridSize - this.Height + Screen.GridSize / 2;

            
        }

        public void Draw()
        {
            FrontSpriteBatch.Draw(NPCimg, new Rectangle((int)(this.X - Screen.CameraX), (int)(this.Y - Screen.CameraY), (int)this.Width, (int)this.Height), Color.White);
        }

        public void Update()
        {
            Hitbox = new Rectangle((int)(this.X + this.HitboxWidthMargin), (int)(this.Y + this.Height - this.HitboxHeight), (int)this.HitboxWidth, (int)this.HitboxHeight);

            if (Moving == true)
            {

                Tuple<int, int> CurrentTile = Screen.GridCords(this.Xnormalized, this.Ynormalized);

                //test if the NPC reached his goal of the indivual tiles (NPC walks from tile to tile)

                if (this.X_towalk <= 0 && this.Y_towalk <= 0)
                {
                    this.Vertical_Momentum = 0;
                    this.Horizontal_Momentum = 0;

                    this.TilesWalked += 1;

                    if (this.TilesWalked != Path.Count() - 1)
                    {
                        Console.WriteLine(Path.Count() + " - > " + TilesWalked);
                        this.Walking();
                    }
                    else
                    {
                        Moving = false;
                    }
                }
                else
                {
                    this.X += this.Horizontal_Momentum;
                    this.Y += this.Vertical_Momentum;

                    this.X_towalk -= Math.Abs(this.Horizontal_Momentum);
                    this.Y_towalk -= Math.Abs(this.Vertical_Momentum);
                }
            }

        }

        public void Walking()
        {

            //Here the Tiles withing the Path are converted into actual Movement

            this.Destination = Screen.ScreenCords(Path[TilesWalked + 1].X, Path[TilesWalked + 1].Y);

            int DirectionX = Path[TilesWalked].X - Path[TilesWalked + 1].X;
            int DirectionY = Path[TilesWalked].Y - Path[TilesWalked + 1].Y;

            if (DirectionX < 0)
            {
                this.Horizontal_Momentum = 3;
                X_towalk = Screen.GridSize;
            }
            else if(DirectionX > 0)
            {
                this.Horizontal_Momentum = -3;
                X_towalk = Screen.GridSize;
            }
            else
            {
                X_towalk = 0;
            }
            if (DirectionY < 0)
            {
                this.Vertical_Momentum = 2;
                Y_towalk = Screen.GridSize;
            }
            else if(DirectionY > 0)
            {
                this.Vertical_Momentum = -2;
                Y_towalk = Screen.GridSize;
            }
            else
            {
                Y_towalk = 0;
            }


        }

        public void CalculatePath(int XGoal, int YGoal)
        {
            //THIS DOESNT WORK AND WIL NOT BE USED

            this.TilesWalked = 0;
            this.Moving = true;

            List<PathData> WrongTiles = new List<PathData>();

            int DirectionX = 0;
            String S_DirectionX = "";
            int DirectionY = 0;
            String S_DirectionY = "";
            string Direction = "";

            int CurrentX = this.Xtile;
            int CurrentY = this.Ytile;

            int NextX = CurrentX;
            int NextY = CurrentY;

            Path.Add(new PathData(CurrentX, CurrentY));
            

            int AmountX = this.Xtile - XGoal;
            int AmountY = this.Ytile - YGoal;

            if (AmountX >= 0) { DirectionX = -1; S_DirectionX = "Left"; }
            else if (AmountX < 0) { DirectionX = 1; S_DirectionX = "Right"; }
            //else { DirectionX = 0; }
            if (AmountY >= 0) { DirectionY = -1; S_DirectionY = "Up"; }
            else if (AmountY < 0) { DirectionY = 1; S_DirectionY = "Down"; }
            //else { DirectionY = 0; }

            AmountX = Math.Abs(AmountX);
            AmountY = Math.Abs(AmountY);

            

            while (AmountX != 0 || AmountY != 0)
            {
                bool TileLeft = (Tilelist.Any(x => x.Y == CurrentY && x.X == (CurrentX + -1) && x.Solid == false) && !WrongTiles.Any(x => x.Y == CurrentY && x.X == (CurrentX + -1)) && !Path.Any(x => x.Y == CurrentY && x.X == (CurrentX + -1)));
                bool TileRight = (Tilelist.Any(x => x.Y == CurrentY && x.X == (CurrentX + 1) && x.Solid == false) && !WrongTiles.Any(x => x.Y == CurrentY && x.X == (CurrentX + 1)) && !Path.Any(x => x.Y == CurrentY && x.X == (CurrentX + 1)));
                bool TileUp = (Tilelist.Any(x => x.Y == (CurrentY + -1) && x.X == CurrentX && x.Solid == false) && !WrongTiles.Any(x => x.Y == (CurrentY + -1) && x.X == CurrentX) && !Path.Any(x => x.Y == (CurrentY + -1) && x.X == CurrentX)) ;
                bool TileDown = (Tilelist.Any(x => x.Y == (CurrentY + 1) && x.X == CurrentX && x.Solid == false) && !(WrongTiles.Any(x => x.Y == (CurrentY + 1) && x.X == CurrentX)) && !Path.Any(x => x.Y == (CurrentY + 1) && x.X == CurrentX));

                Console.WriteLine(TileLeft + " " + TileRight + " " + TileUp + " " + TileDown);

                if (TileLeft == false && TileRight == false && TileUp == false && TileDown == false)
                {
                    TileLeft = (Tilelist.Any(x => x.Y == CurrentY && x.X == (CurrentX + -1) && x.Solid == false) && !WrongTiles.Any(x => x.Y == CurrentY && x.X == (CurrentX + -1)));
                    TileRight = (Tilelist.Any(x => x.Y == CurrentY && x.X == (CurrentX + 1) && x.Solid == false) && !WrongTiles.Any(x => x.Y == CurrentY && x.X == (CurrentX + 1)));
                    TileUp = (Tilelist.Any(x => x.Y == (CurrentY + -1) && x.X == CurrentX && x.Solid == false) && !WrongTiles.Any(x => x.Y == (CurrentY + -1) && x.X == CurrentX));
                    TileDown = (Tilelist.Any(x => x.Y == (CurrentY + 1) && x.X == CurrentX && x.Solid == false) && !(WrongTiles.Any(x => x.Y == (CurrentY + 1) && x.X == CurrentX)));

                }

                // Console.WriteLine(AmountX);


                if (AmountX >= AmountY)
                {
                    if ((S_DirectionX == "Left" && TileLeft == true) || (S_DirectionX == "Right" && TileRight == true))
                    {
                        Direction = S_DirectionX;
                        AmountX -= 1;
                    }
                    else if ((S_DirectionY == "Up" && TileUp == true) || (S_DirectionY == "Down" && TileDown == true))
                    {
                        Direction = S_DirectionY;
                        AmountY -= 1;
                    }
                    else if ((S_DirectionY == "Up" && TileDown == true) || (S_DirectionY == "Down" && TileUp == true))
                    {
                        if (S_DirectionY == "Up" && TileDown == true) { Direction = "Down"; }
                        else if (S_DirectionY == "Down" && TileUp == true) { Direction = "Up"; }
                        AmountY += 1;
                    }
                    else if ((S_DirectionX == "Left" && TileRight == true) || (S_DirectionX == "Right" && TileLeft == true))
                    {
                        if (S_DirectionX == "Left" && TileRight == true) { Direction = "Right"; }
                        else if (S_DirectionX == "Right" && TileLeft == true) { Direction = "Left"; }
                        AmountX += 1;
                    }

                }
                else if (AmountY > AmountX)
                {
                    if ((S_DirectionY == "Up" && TileUp == true) || (S_DirectionY == "Down" && TileDown == true))
                    {
                        Direction = S_DirectionY;
                        AmountY -= 1;
                    }
                    else if ((S_DirectionX == "Left" && TileLeft == true) || (S_DirectionX == "Right" && TileRight == true))
                    {
                        Direction = S_DirectionX;
                        AmountX -= 1;
                    }
                    else if ((S_DirectionX == "Left" && TileRight == true) || (S_DirectionX == "Right" && TileLeft == true))
                    {
                        if (S_DirectionX == "Left" && TileRight == true) { Direction = "Right"; }
                        else if (S_DirectionX == "Right" && TileLeft == true) { Direction = "Left"; }
                        AmountX += 1;
                    }
                    else if ((S_DirectionY == "Up" && TileDown == true) || (S_DirectionY == "Down" && TileUp == true))
                    {
                        if (S_DirectionY == "Up" && TileDown == true) { Direction = "Down"; }
                        else if (S_DirectionY == "Down" && TileUp == true) { Direction = "Up"; }
                        AmountY += 1;
                    }
                }

                if (Direction == "Left" || Direction == "Right")
                {
                    NextX = CurrentX + DirectionX;
                }
                else if(Direction == "Up" || Direction == "Down")
                {
                    NextY = CurrentY + DirectionY;
                }

                if (Path.Any(x => x.X == NextX && x.Y == NextY))
                {
                    WrongTiles.Add(new PathData(CurrentX, CurrentY));
                    int Index = Path.FindIndex(x => x.X == NextX && x.Y == NextY);
                    Path = new List<PathData>(Path.Take(Index));
                }
                else
                {
                    Path.Add(new PathData(NextX, NextY));
                }


                if (AmountX < 0)
                {
                    AmountX = Math.Abs(AmountX);
                    if (S_DirectionX == "Left")
                    {
                        S_DirectionX = "Right";
                        DirectionX = 1;
                    }
                    else
                    {
                        S_DirectionX = "Left";
                        DirectionX = -1;
                    }
                }
                if (AmountY < 0)
                {
                    AmountY = Math.Abs(AmountY);
                    if (S_DirectionY == "Down")
                    {
                        S_DirectionY = "Up";
                        DirectionY = -1;
                    }
                    else
                    {
                        S_DirectionY = "Down";
                        DirectionY = 1;
                    }
                }
   
                CurrentX = NextX;
                CurrentY = NextY;

                Console.WriteLine(CurrentX + "  --  " + CurrentY);



            }

            foreach( var Tile in Path)
            {
                //Console.WriteLine(Tile.X + " -- " + Tile.Y);
            }

        }
    }

}

        public struct PathData
        {
            public PathData(int X, int Y)
            {
                this.X = X;
                this.Y = Y;
            }

            public int X { get; private set; }
            public int Y { get; private set; }
        }
