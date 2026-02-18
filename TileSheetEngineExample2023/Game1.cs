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
        bool level;


        private Camera cam;

        private TileLayer t_layer;

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
        Vector2 teleporterPos = new Vector2(8,30);
        
        int[,] tileMap = new int[,]
            {
                {1,0,2,2,2,2,1,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
                {1,1,2,2,2,2,1,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
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
        

            int[,] tileMap2 = new int[,]
            {
                {2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
                {2,1,1,1,1,1,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
                {2,1,1,1,1,1,1,2,2,1,1,1,1,1,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
                {2,1,1,0,0,1,1,1,1,1,1,0,0,1,1,1,1,1,1,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
                {2,1,1,0,0,1,1,1,1,1,1,0,0,1,1,1,1,1,1,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
                {2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,2,2,1,1,1,1,1,1,1,1,1,1,1,1,1,2},
                {2,2,2,2,2,2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,1,1,2},
                {2,2,2,2,2,2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,1,1,2},
                {2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2},
                {2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2},
                {2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2}
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
            cam = new Camera(graphics.PreferredBackBufferHeight, graphics.PreferredBackBufferWidth);
            this.Services.AddService(cam);

            tileMap[(int)teleporterPos.X, (int)teleporterPos.Y] = 3;
            // Create a new SpriteBatch, which can be used to draw textures.
            Helper.SpriteSheet = Content.Load<Texture2D>("tank tiles 64 x 64");
            spriteBatch = new SpriteBatch(GraphicsDevice);
            spriteFont = Content.Load<SpriteFont>("gameFont");
            player = new TiledPlayer(
                new Vector2(64, 64),
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
                64, 64, 1.0f, this);
            
            // TODO: use this.Content to load your game content here
            InitTiMap(tileMap);
            SpawnPlayer(tileMap);

        }

        private void InitTiMap(int[,] tile)
        {
            colliders.Clear();
            tile[(int)teleporterPos.X, (int)teleporterPos.Y] = 3;
            foreach (TileTypes type in ImpassableTileTypes)
            {
                SetColliders(type, tile);
            }
            teleporter = new Collider(Content.Load<Texture2D>(@"collider"), (int)teleporterPos.Y, (int)teleporterPos.X);

            t_layer = new TileLayer(tile, layer_tileRefs, 64, 64);
        }

        public void SetColliders(TileTypes t, int[,] tile)
        {
            for (int x = 0; x < tile.GetLength(1); x++)
                for (int y = 0; y < tile.GetLength(0); y++)
                {
                    if (tile[y, x] == (int)t)
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
                if (level)
                {
                    Exit();
                }
                InitTiMap(!level ? tileMap2 : tileMap);
                SpawnPlayer(!level ? tileMap2 : tileMap);
                level = !level;
            }
            cam.Follow(player);
            

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
            spriteBatch.DrawString(spriteFont, "Arthur Menudier\nS00295662", new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2), Color.White);
            spriteBatch.DrawString(spriteFont, "Arthur Menudier\nS00295662", new Vector2(0, 0), Color.White);
            spriteBatch.End();
            spriteBatch.Begin(transformMatrix: cam.Transform);
            t_layer.Draw(spriteBatch);
            player.Draw(spriteBatch, Helper.SpriteSheet);
            // TODO: Add your drawing code here

            spriteBatch.End();
            base.Draw(gameTime);
        }

        private void SpawnPlayer(int[,] tile)
        {
            for (int i = 0; i < tile.GetLength(0); i++)
            {
                for(int j = 0; j < tile.GetLength(1); j++)
                {
                    if (tile[i,j] == 1)
                    {
                        player.PixelPosition = new Vector2(64 * i, 64 * j);
                        player.AngleOfRotation = 0;
                        return;
                    }
                }
            }
        }
    }
}
