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
    class Textbox
    {
        public Vector2 Location { get; set; }
        public string FullText { get; set; }
        public string OnScreenText { get; set; }

        public int CharPerLine = 50;

        private List<string> Lines { get; set; }
        private List<string> AllLines { get; set; }
        public int SizeX { get; set; }
        public int SizeY { get; set; }

        private List<string> LinesOnscreen { get; set; }
        private int OnPage { get; set; }
        private int OnLine { get; set; }
        private int OnLetter { get; set; }
        public bool SkipText { get; set; }
        public bool Continue { get; set; }
        public bool Finish { get; set; }

        private Texture2D TextBoximg { get; set; }
        private SpriteBatch FrontSpriteBatch;
        private PrologueContent prologueContent { get; set; }

        public Rectangle Hitbox { get; set; }

        private int _Timer { get; set; }

        public Textbox(Vector2 Location, String FullText, int SizeX, int SizeY, SpriteBatch FrontSpriteBatch, PrologueContent prologueContent)
        {
            //this.Location = Location;
            this.FullText = FullText;

            this.SizeX = SizeX;
            this.SizeY = SizeY;

            this.Location = new Vector2(Screen.ScreenWidth / 2 - this.SizeX / 2, Screen.ScreenHeight / 2 + 130);

            this.prologueContent = prologueContent;
            this.FrontSpriteBatch = FrontSpriteBatch;
            this.TextBoximg = prologueContent.Tile1;

            LinesOnscreen = new List<string> { "", "", "", "" };
            this.OnPage = 0;
            this.OnLine = 0;
            this.OnLetter = 0;
            this.SkipText = false;
            this.Continue = true;
            this.Finish = false;
            Hitbox = new Rectangle((int)this.Location.X, (int)this.Location.Y, this.SizeX, this.SizeY);

            _Timer = 0;
        }

        public void SplitInLines()
        {
            //Here the Text is first split up in lines where ever the programmer put a @, But if the text is stil longer then the MaxChar that fits in the width of the textbox
            // It is split down even further

            Lines = FullText.Split((char)'@').ToList();
            AllLines = new List<string> { };

            foreach (string _Line in Lines)
            {
                string Templine =  "" ;
                Templine = _Line;

                if (_Line.Count() > CharPerLine)
                {
                    int _AmountOfLines = _Line.Count() / CharPerLine;
                    int x = 0;
                    int CurrentIndex = CharPerLine;
                    while (_AmountOfLines > 0)
                    {
                        if (_Line.ElementAtOrDefault(CurrentIndex) == ' ')
                        {
                            StringBuilder sb = new StringBuilder(Templine);
                            sb[CurrentIndex] = '@';
                            Templine = sb.ToString();
                            _AmountOfLines -= 1;

                            CurrentIndex += CharPerLine;
                        }
                        else
                        {
                            x++;
                            CurrentIndex -= 1;
                        }
                    }
                    AllLines.AddRange(Templine.Split((char)'@').ToList());
                }
                else
                {
                    AllLines.Add(_Line);
                }
            }

            foreach(string y in AllLines)
            {
                Console.WriteLine(y);
            }

       
            FullText = FullText.Replace("@", System.Environment.NewLine);

        }

        public void TextBoxUpdate()
        {

            //General Logic for properly displaying all Text Lines Combined with a Slow mode, so the text isnt instant

            if (this.AllLines.Count == this.OnLine + this.OnPage * 4)
            {
                this.Finish = true;
            }
            if (this.Finish == false && (_Timer % 4 == 0 || this.SkipText == true))
            {
                this._Timer = 0;
                if (LinesOnscreen[this.OnLine].Length == this.AllLines[this.OnLine + this.OnPage * 4].Length)
                {
                    if (OnLine == 3)
                        this.Continue = false;
                    else { OnLine += 1; OnLetter = 0; }

                }
                else if (this.Continue == true)
                {
                    LinesOnscreen[this.OnLine] = LinesOnscreen[OnLine] + (AllLines[this.OnLine + this.OnPage * 4][this.OnLetter]);
                    OnLetter += 1;
                }
            }


            _Timer += 1;
        }

        public void NextPage()
        {

            //If the OnLine variable = 4, Aka the page is full, the game wil wait for PlayerInput to go to the next page, this Page switching happens here.
            if (this.Continue == false)
            {
                this.Continue = true;
                Console.WriteLine(OnPage);
                this.OnLine = 0;
                this.OnPage += 1;
                this.OnLetter = 0;
                this.SkipText = false;

                LinesOnscreen = new List<string>{ "","","",""};
                    
            }
        }

        public void Draw()
        {
            FrontSpriteBatch.Draw(TextBoximg, new Rectangle((int)(this.Location.X), (int)(this.Location.Y), SizeX, SizeY), Color.Green);
            FrontSpriteBatch.DrawString(prologueContent.Arial20, LinesOnscreen[0], new Vector2(220, 440), Color.White);
            FrontSpriteBatch.DrawString(prologueContent.Arial20, LinesOnscreen[1], new Vector2(220, 480), Color.White);
            FrontSpriteBatch.DrawString(prologueContent.Arial20, LinesOnscreen[2], new Vector2(220, 520), Color.White);
            FrontSpriteBatch.DrawString(prologueContent.Arial20, LinesOnscreen[3], new Vector2(220, 560 ), Color.White);
        }
    }

    //Same as with the objects, Different Kind of TextBox wil have different Functions, so a different class.

    class InformationTextBox : Textbox
    {
        public InformationTextBox(Vector2 Location,String FullText, int SizeX, int SizeY, SpriteBatch FrontSpriteBatch, PrologueContent prologueContent) : base(Location, FullText, SizeX, SizeY, FrontSpriteBatch, prologueContent)
        {
        }

    }

 /*   class SpeakingTextBox : Textbox
    {

    } */

  /*  class QuestionTextBox : Textbox
    {

    } */
}
