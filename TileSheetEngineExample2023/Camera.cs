using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TiledSpriteExample;
namespace Cameras
{
    public class Camera
    {
        public Matrix Transform { get; private set; }
        private int _height;
        private int _width;
        public Camera(int height, int witdh) { 
            _height = height;
            _width = witdh;
        }
        public void Follow(TiledPlayer target)
        {
            int size = 1;
            var position = Matrix.CreateTranslation(
              -target.PixelPosition.X * size,
              -target.PixelPosition.Y *size,
              0);
            var offset = Matrix.CreateTranslation(
                _width / 2,
                _height / 2,
                0);
            Transform = position * offset;
        }

    }
}
