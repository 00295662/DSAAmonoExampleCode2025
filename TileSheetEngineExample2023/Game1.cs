using Cameras;
using Engine.Engines;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using TiledSpriteExample;
using Tiler;
using Tracker.WebAPIClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;

namespace TileSheetEngineExample2023
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    /// 
    public enum TileTypes {STEEL_WALL_TILE,STEEL_FLOOR_TILE,BLUE_STEEL_WALL_TILE,EAGLE_TILE }
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont spriteFont;
        TiledPlayer player;
        TileLayer current_layer;

        private Camera cam;

        private TileLayer t_layer;
        TileLayer level2;

        List<TileRef> layer_tileRefs = new List<TileRef>()
        {
            // column, row, value in TileMap
            new TileRef(0,1,(int)TileTypes.STEEL_WALL_TILE),
            new TileRef(3,3,(int)TileTypes.STEEL_FLOOR_TILE),
            new TileRef(4,2,(int)TileTypes.BLUE_STEEL_WALL_TILE),
            new TileRef(0,2,(int)TileTypes.EAGLE_TILE),
            // Include Special Tile here
        };
        // Just for future reference Not used here
        TileTypes[] ImpassableTileTypes = new TileTypes[] { TileTypes.BLUE_STEEL_WALL_TILE, TileTypes.STEEL_WALL_TILE};
        List<Collider> colliders = new List<Collider>();
        Collider teleporter;
        Vector2 teleporterPos = new Vector2(2,6);

        int[,] tileMap = new int[,]
            {
                {1,0,2,2,2,2,1,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
                {1,2,2,2,2,2,1,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
                {1,1,1,1,1,1,1,1,1,1,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
                {1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,2,2,2,2,2,2,2},
                {2,2,2,2,2,2,0,0,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,2,2,2,2,2,2,2},
                {2,2,2,2,2,2,0,0,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,2,2,2,2,2,2,2},
                {2,2,2,2,2,2,0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,2,0,0,0,0,0,2,0,0,0,0,2,0,0,0,2,0,0,0,2,2,2,2,2,2,2,2,2},
                {2,2,2,2,2,2,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                {2,2,2,2,2,2,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                {2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1,1,2,2,2,1,1,2,2,2,2,1,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
                {2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1,1,2,2,2,1,1,2,2,2,2,1,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
                {2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1,1,1,1,1,1,1,1,1,1,1,1,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
                {2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1,1,2,2,2,1,1,2,2,2,2,1,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
                {2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1,1,2,2,2,1,1,2,2,2,2,1,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
                {2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1,1,2,2,2,1,1,2,2,2,2,1,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
                {2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1,1,1,1,1,1,1,1,1,1,1,1,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
                {2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1,1,1,1,1,1,1,1,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
                {2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
                {2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
            };
        public Game1()
        {
            IsMouseVisible = true;
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
            ActivityAPIClient.Track(StudentID: "S00295662", StudentName: "Arthur Menudier", activityName:" DSAA 2026 Week 5 Lab 2", Task: " Week 5 Lab 2 Implementing level selection");
            // TODO: Add your initialization logic here
            new InputEngine(this);
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            cam = new Camera(new Vector2(10,10), new Vector2(10000,10000));
            this.Services.AddService(cam);

            tileMap[(int)teleporterPos.X, (int)teleporterPos.Y] = 3;
            // Create a new SpriteBatch, which can be used to draw textures.
            Helper.SpriteSheet = Content.Load<Texture2D>("tank tiles 64 x 64");
            spriteBatch = new SpriteBatch(GraphicsDevice);
            spriteFont = Content.Load<SpriteFont>("gameFont");
            player = new TiledPlayer(
                new Vector2(0,0),
                new List<TileRef>()
                {
                    new TileRef(15,9,0),
                    new TileRef(16,9,0),
                    new TileRef(17,9,0),
                    new TileRef(18,9,0),
                    new TileRef(19,9,0),
                    new TileRef(20,9,0),
                    new TileRef(21,9,0),
                }, 
                64, 64, 1.0f,this);
            t_layer = new TileLayer(tileMap,layer_tileRefs, 64,64);
            // TODO: use this.Content to load your game content here
            tileMap[(int)teleporterPos.X, (int)teleporterPos.Y] = 3;
            foreach (TileTypes type in ImpassableTileTypes) {
                SetColliders(type);
            }
            teleporter = new Collider(Content.Load<Texture2D>(@"collider"), (int)teleporterPos.Y,(int)teleporterPos.X);


        }
        public void SetColliders(TileTypes t)
        {
            for (int x = 0; x < tileMap.GetLength(1); x++)
                for (int y = 0; y < tileMap.GetLength(0); y++)
                {
                    if (tileMap[y, x] == (int)t)
                    {
                        colliders.Add(new Collider(
                            Content.Load<Texture2D>(@"collider"),
                            x,y
                            ));
                    }

                }
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
            
            player.Update(gameTime);
            // TODO: Add your update logic here

            foreach (var c in colliders)
            {
                player.Collision(c);
            }
            if (player.Collide(teleporter))
            {
                player.PixelPosition = new Vector2(0, 0);
                player.AngleOfRotation = 0;
            }
            

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            t_layer.Draw(spriteBatch);
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, null, Camera.CurrentCameraTranslation);
            player.Draw(spriteBatch, Helper.SpriteSheet);
            // TODO: Add your drawing code here
            spriteBatch.DrawString(spriteFont, "Arthur Menudier\nS00295662", new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2), Color.White);
            spriteBatch.DrawString(spriteFont, "Arthur Menudier\nS00295662", new Vector2(0, 0), Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
