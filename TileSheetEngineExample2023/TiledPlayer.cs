using Engine.Engines;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tiler;
using Cameras;

namespace TiledSpriteExample
{
    public class TiledPlayer : TiledSprite
    {
        public Rectangle CollisionField
        {
            get
            {
                return new Rectangle(pixelPosition.ToPoint(),
                new Point(FrameWidth, FrameHeight));
            }

        }
        public float RotationSpeed { get; set; }
        public float speed { get; set; }
        private Camera cam;
        private Game _game;
        public TiledPlayer(Vector2 tilePosition, 
            List<TileRef> sheetRefs, int frameWidth, 
            int frameHeight, float layerDepth,Game game) : base(tilePosition, sheetRefs, frameWidth, frameHeight, layerDepth)
        {
            speed = 5.0f;
            RotationSpeed = 0.02f;
            cam = game.Services.GetService<Camera>();
            _game = game;
        }

        Vector2 previousPixelPosition;
        public override void Update(GameTime gametime)
        {
            previousPixelPosition = pixelPosition;
            if (InputEngine.IsKeyHeld(Keys.X))
                AngleOfRotation += RotationSpeed;
            if (InputEngine.IsKeyHeld(Keys.Z))
                AngleOfRotation -= RotationSpeed;

            if (InputEngine.IsKeyHeld(Keys.Up))
                PixelPosition += new Vector2(
                    (float)Math.Sin(AngleOfRotation),
                    -(float)Math.Cos(AngleOfRotation)) * 5;

            if (InputEngine.IsKeyHeld(Keys.Down))
                PixelPosition -= new Vector2(
                    (float)Math.Sin(AngleOfRotation),
                    -(float)Math.Cos(AngleOfRotation)) * 5;



            Camera.follow(pixelPosition*64, _game.GraphicsDevice.Viewport);

            base.Update(gametime);
        }

        public void Collision(Collider c)
        {
            if (CollisionField.Intersects(c.CollisionField))
                pixelPosition = previousPixelPosition;
        }
        public bool Collide(Collider c)
        {
            return CollisionField.Intersects(c.CollisionField);
        }

        public override void Draw(SpriteBatch spriteBatch, Texture2D SpriteSheet)
        {
            base.Draw(spriteBatch, SpriteSheet);
        }

    }
}
