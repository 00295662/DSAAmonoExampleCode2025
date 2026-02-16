using Engine.Engines;
using GameComponentExample2025;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Sprites.Player;

namespace Sprites
{
    public class Collectible : Sprite
    {
        SpriteFont _font;
        Random _random;

        SoundEffect _soundEffect;
        SoundEffectInstance _soundPlayer;

        SpriteBatch _spriteBatch;

        Player _player;

        int _value = 0;

        bool _opened = false;

        public Collectible(Game game, Texture2D texture,SoundEffect soundEffect,SpriteFont spriteFont, Vector2 userPosition, int framecount) : base(game, texture, userPosition, framecount)
        {
            
            _player = game.Services.GetService<Player>();
            _spriteBatch = game.Services.GetService<SpriteBatch>();
            _soundEffect = soundEffect;
            _font = spriteFont;
            _random = new Random();
        }
        public override void Update(GameTime gametime)
        {
            base.Update(gametime);
            if(_opened) return;
                if (collisionDetect(_player)) {
                    _soundPlayer = _soundEffect.CreateInstance();
                    _soundPlayer.Play();
                    _opened = true;
                    _value = _random.Next(20,50);
                    Game1.Collect(this, _value);
                }
            
        }
        public override void Draw(GameTime gametime)
        {
            if (_opened) return;
            base.Draw(gametime);
        }
    }
}
