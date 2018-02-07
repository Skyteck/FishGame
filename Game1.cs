using FishGame.GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace FishGame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Fish fish;
        List<FoodPellet> PelletList;
        List<Bubble> BubbleList;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
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
            this.IsMouseVisible = true;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            

            fish = new Fish();
            fish.LoadContent(@"Art/Fishy", Content);

            fish._Position = new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);

            PelletList = new List<FoodPellet>();
            BubbleList = new List<Bubble>();
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

            if (this.IsActive != true) return;

            InputHelper.Update();

            // TODO: Add your update logic here
            ProcessMouse(gameTime);
            fish.Update(gameTime, PelletList);
            foreach(FoodPellet fp in PelletList)
            {
                fp.Update(gameTime);


            }

            Random ran = new Random();
            int spawnBubble = ran.Next(0, 30);

            if(spawnBubble == 1 || spawnBubble == 15)
            {
                Bubble b = new Bubble();
                b.LoadContent(@"Art/Bubble", Content);
                b._Position.X = ran.Next(0, 800);
                b._Position.Y = ran.Next(500, 1200);
                BubbleList.Add(b);
            }

            foreach(Bubble b in BubbleList)
            {
                b.Update(gameTime);
            }

            base.Update(gameTime);
        }

        private void ProcessMouse(GameTime gt)
        {
            if(InputHelper.LeftButtonClicked)
            {
                FoodPellet np = new FoodPellet();
                np.LoadContent(@"Art/SlimeShot", Content);
                np._Position = InputHelper.MouseScreenPos;
                PelletList.Add(np);
            }

            if(InputHelper.IsKeyDown(Keys.LeftControl) && InputHelper.IsKeyDown(Keys.LeftShift) && InputHelper.LeftButtonClicked)
            {
                Console.WriteLine(InputHelper.MouseScreenPos);
            }
        }
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkBlue);
            spriteBatch.Begin();
            // TODO: Add your drawing code here
            fish.Draw(spriteBatch);

            foreach (FoodPellet fp in PelletList)
            {
                fp.Draw(spriteBatch);
            }


            foreach (Bubble b in BubbleList)
            {
                b.Draw(spriteBatch);
            }

            base.Draw(gameTime);

            spriteBatch.End();
        }
    }
}
