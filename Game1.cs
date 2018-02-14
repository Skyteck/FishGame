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
        List<Waterline> WaterLineSprites;
        double BubbleTime = 0;
        Random ran;

        Texture2D floorTex;
        Texture2D bgTex;
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
            ran = new Random();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);


            floorTex = Content.Load<Texture2D>(@"Art/Rocks");
            bgTex = Content.Load<Texture2D>(@"Art/bg");
            fish = new Fish();
            fish.LoadContent(@"Art/Fishy", Content);

            fish._Position = new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);

            PelletList = new List<FoodPellet>();
            BubbleList = new List<Bubble>();
            WaterLineSprites = new List<Waterline>();

            for(int i = -1; (i < (GraphicsDevice.Viewport.Width/32) + 3); i++)
            {
                Waterline nl = new Waterline();
                nl._Position.X = 32 * i;
                nl._Position.Y = 70;
                nl.LoadContent(@"Art/Waterline", Content);
                WaterLineSprites.Add(nl);
            }
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

            BubbleTime += gameTime.ElapsedGameTime.TotalSeconds;

            double BubblesPerSec = 6;
            double bubbleTimer = (1.0 / BubblesPerSec);
            if (BubbleTime > bubbleTimer)
            {
                GetBubble();
                BubbleTime -= bubbleTimer;
                

            }

            foreach(Bubble b in BubbleList)
            {
                b.Update(gameTime);
            }

            foreach(Waterline nl in WaterLineSprites)
            {
                nl.Update(gameTime);
            }

            base.Update(gameTime);
        }

        private void ProcessMouse(GameTime gt)
        {
            if(InputHelper.LeftButtonClicked)
            {
                if(InputHelper.MouseScreenPos.Y < 70)
                {
                    GetFood();
                }
            }

            if(InputHelper.IsKeyDown(Keys.LeftControl) && InputHelper.IsKeyDown(Keys.LeftShift) && InputHelper.LeftButtonClicked)
            {
                Console.WriteLine(InputHelper.MouseScreenPos);
            }

            if(InputHelper.RightButtonHeld)
            {
                GetBubble(InputHelper.MouseScreenPos);
            }
        }
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            Color WaterColor = new Color(204, 176, 148, 255);
            GraphicsDevice.Clear(WaterColor);
            spriteBatch.Begin();
            // TODO: Add your drawing code here


            spriteBatch.Draw(bgTex, new Vector2(0, 70), Color.White);

            foreach (Waterline nl in WaterLineSprites)
            {
                nl.Draw(spriteBatch);
            }

            foreach (Bubble b in BubbleList)
            {
                b.Draw(spriteBatch);
            }


            spriteBatch.Draw(floorTex, new Vector2(0, GraphicsDevice.Viewport.Height - 50), Color.White);

            fish.Draw(spriteBatch);

            foreach (FoodPellet fp in PelletList)
            {
                fp.Draw(spriteBatch);
            }




            base.Draw(gameTime);

            spriteBatch.End();
        }

        private void GetBubble()
        {
            Bubble b = BubbleList.Find(x => x._CurrentState == Sprite.SpriteState.kStateInActive);

            if(b != null)
            {
                b.Activate(new Vector2(ran.Next(0, 800), ran.Next(500, 600)));
            }
            else
            {
                b = new Bubble();
                b.LoadContent(@"Art/Bubble", Content);
                b._Position.X = ran.Next(0, 800);
                b._Position.Y = ran.Next(500, 600);
                BubbleList.Add( b);
            }
        }

        private void GetFood()
        {
            FoodPellet np = PelletList.Find(x => x._CurrentState == Sprite.SpriteState.kStateInActive);

            if (np != null)
            {
                np.Activate(InputHelper.MouseScreenPos);
            }
            else
            {
                np = new FoodPellet();
                np.LoadContent(@"Art/SlimeShot", Content);
                np._Position = InputHelper.MouseScreenPos;
                PelletList.Add(np);
            }
        }

        private void GetBubble(Vector2 pos)
        {
            Bubble b = BubbleList.Find(x => x._CurrentState == Sprite.SpriteState.kStateInActive);

            if (b != null)
            {
                b.Activate(pos);
            }
            else
            {
                b = new Bubble();
                b.LoadContent(@"Art/Bubble", Content);
                b._Position.X = ran.Next(0, 800);
                b._Position.Y = ran.Next(500, 600);
                b._Position = pos;
                BubbleList.Add(b);
            }
        }
    }
}
