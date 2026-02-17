using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Cameras
{
    public class Camera
    {
        static Vector2 _camPos = Vector2.Zero;
        static public Vector2 WorldBound;
        public static Matrix CurrentCameraTranslation { get
            {
                return Matrix.CreateTranslation(new Vector3(
                    -CamPos,
                    0));
            } }

        public static Vector2 CamPos
        {
            get
            {
                return _camPos;
            }

            set
            {
                _camPos = value;
            }
        }

        public Camera(Vector2 startPos, Vector2 bound)
        {
            CamPos = startPos;
            WorldBound = bound;
        }

        public void move(Vector2 delta, Viewport v)
        {
            CamPos += delta;
            CamPos = Vector2.Clamp(CamPos, Vector2.Zero, WorldBound - new Vector2(v.Width, v.Height));
        }

        public static void follow(Vector2 followPos, Viewport v)
        {
            _camPos = followPos - new Vector2(v.Width / 2, v.Height / 2);
            _camPos = Vector2.Clamp(_camPos, Vector2.Zero, WorldBound - new Vector2(v.Width, v.Height));
        }

    }
}
