using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Prologue
{


    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch, FrontSpriteBatch;
        PrologueContent prologueContent;

        private MouseState oldMouseState;
        private KeyboardState oldKeyboardState;

        public double GridSizeWidth, GridSizeHeight, GridSize;

        private Map map1;
        private Player player;
        private NPC npc1;
        private NPC npc2;
        private InformationTextBox textbox1;
        private Objects TestObject;

        public bool intersect;

        

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 600;
            graphics.PreferredBackBufferWidth = 1000;
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
            IsMouseVisible = true;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            FrontSpriteBatch = new SpriteBatch(GraphicsDevice);

            prologueContent = new PrologueContent(Content);

            //Some general Setup information

            Screen.ScreenWidth = GraphicsDevice.Viewport.Width;
            Screen.ScreenHeight = GraphicsDevice.Viewport.Height;

            Screen.GridSize = Screen.ScreenWidth / Screen.MinGridX;

            map1 = new Map(prologueContent, spriteBatch);
            player = new Player(350, 180, FrontSpriteBatch, prologueContent, map1.Tilelist);
            npc1 = new NPC(3, 5, FrontSpriteBatch, prologueContent, map1.Tilelist);

            Vector2 Location = new Vector2(100, 100);
            textbox1 = new InformationTextBox(Location, "HALLO HOE GAAT ET ERMEE@ Persoonlijk gaat het wel redelijk met mij, de bedoeling van deze zin is is dat ie tering lang word zodat hij gesplit moet worden@ Maar.. Maar@ Ohhnee@stut@stut@stutterrr@Jahoor@volgende pagina aub dankuwel@", 600, 150, FrontSpriteBatch, prologueContent);
            textbox1.SplitInLines();

            TestObject = new Objects(5, 5, "KEK", FrontSpriteBatch, prologueContent);




            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            KeyboardState newKeyboardState = Keyboard.GetState();
            MouseState newMouseState = Mouse.GetState();

            //Keyboard Input Registration 
            if (newKeyboardState.IsKeyDown(Keys.A))
            {
                player.HorizontalMov("Left");
            }
            if (newKeyboardState.IsKeyDown(Keys.D))
            {
                player.HorizontalMov("Right");
            }
            if (newKeyboardState.IsKeyDown(Keys.W))
            {
                player.VerticalMov("Up");
            }
            if (newKeyboardState.IsKeyDown(Keys.S))
            {
                player.VerticalMov("Down");
            }
            if (newKeyboardState.IsKeyDown(Keys.Q))
            {
                if (textbox1.Continue == false)
                {
                    textbox1.SkipText = false;
                    textbox1.NextPage();
                }
                else { textbox1.SkipText = true; }
            }
            
            //-----------------------------

            oldKeyboardState = newKeyboardState;

            //Updating of all the classes, This is a temperary Test setup
            player.Update();
            npc1.Update();
            textbox1.TextBoxUpdate();
            TestObject.Update(player.Hitbox);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();
            foreach(var Tile in map1.Tilelist)
            {
                Tile.Draw(Screen.GridSize);
            }
            spriteBatch.End();

            FrontSpriteBatch.Begin();
            player.Draw();
            npc1.Draw();
            TestObject.Draw();
            textbox1.Draw();
            FrontSpriteBatch.End();
            



            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }


    }
}
