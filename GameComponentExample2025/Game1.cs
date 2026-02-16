using Cameras;
using Engine.Engines;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Sprites;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
using Tracker.WebAPIClient;


namespace GameComponentExample2025
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont spriteFont;
        private Texture2D _background;
        private Camera cam;

        private static int _score = 0;

        private static List<Collectible> collectibles = new List<Collectible>();

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
            ActivityAPIClient.Track(StudentID: "S0029562", StudentName: "Arthur Menudier", activityName: "DSAA Week 5 Labs 2026", Task: " Week 5 Lab 1 \r\nCreating Game play");
            // TODO: Add your initialization logic here
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteFont = Content.Load<SpriteFont>("gameFont");
            _background = Content.Load<Texture2D>(@"backgroundImage");
            new Sprite(this, _background, Vector2.Zero, 1);
            new InputEngine(this);
            cam = new Camera(Vector2.Zero, _background.Bounds.Size.ToVector2());
            this.Services.AddService(cam);
            spriteBatch = new SpriteBatch(GraphicsDevice);
            this.Services.AddService(spriteBatch);

            SoundEffect[] _PlayerSounds = new SoundEffect[5];
            for (int i = 0; i < _PlayerSounds.Length; i++)
                _PlayerSounds[i] =
                    Content.Load<SoundEffect>(@"Audio/"
                        + i.ToString());


            new Player(this, new Texture2D[] {Content.Load<Texture2D>(@"Images/left"),
                                                Content.Load<Texture2D>(@"Images/right"),
                                                Content.Load<Texture2D>(@"Images/up"),
                                                Content.Load<Texture2D>(@"Images/down"),
                                                Content.Load<Texture2D>(@"Images/stand")},
                _PlayerSounds,
                    new Vector2(200, 200), 6, 0, 5.0f);

            Texture2D[] badge = new Texture2D[14];
            SoundEffect[] sounds = new SoundEffect[14];
            Random rand = new Random();
            for (int i = 0;i < 14; i++)
            {
                badge[i] = Content.Load<Texture2D>(@"Badge Textures/Badges_" + i.ToString());
                sounds[i] = Content.Load<SoundEffect>(@"Badge Audio/Badges_" + i.ToString());
                Vector2 pos = new Vector2(rand.Next(0,_background.Width),rand.Next(0,_background.Height));
                Collectible val = new Collectible(this, badge[i], sounds[i], spriteFont, pos, 1);


                collectibles.Add(val);
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

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            

            // TODO: Add your drawing code here

            base.Draw(gameTime);
            spriteBatch.Begin();
            spriteBatch.DrawString(spriteFont, $"Collectible : {collectibles.Count}\nScore : {_score}", Vector2.Zero, Color.White);
            spriteBatch.End();
        }

        public static void Collect(Collectible collectible,int score)
        {
            collectibles.Remove(collectible);
            _score += score;
        }
    }
}
