using CollisionExample.Collisons;
using DungeonDweller.Archetecture;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct2D1.Effects;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonDweller.Sprites
{
    public class FlameKey : ISprite
    {
        public Vector2 Position { get; set; }

        public Texture2D _texture;

        public Vector2 TilePosition
        {
            get
            {

                return Position / 64;
            }
            set
            {


                Position = value * 64;

            }
        }

        // A timer variable for sprite animation
        private double _animationTimer;

        // The current animation frame 
        private short _animationFrame;

        public BoundingRectangle Bounds => new BoundingRectangle(new(Position.X + 8, Position.Y + 8), 32, 32);

        public string Name => "FlameKey";

        public bool Collected = false;

        public bool Collides(ISprite other)
        {
            return Bounds.CollidesWith(other.Bounds) && !Collected;

        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

            _animationTimer += gameTime.ElapsedGameTime.TotalSeconds;


            if (_animationTimer > (1 / 1.99))
            {
                _animationFrame++;
                if (_animationFrame > 3) _animationFrame = 0;
                _animationTimer -= (1 / 1.99);
            }

            // Determine the source rectangle 
            var sourceRect = new Rectangle(_animationFrame * 64 * 1, 0, 64 * 1, 64 * 1);

            spriteBatch.Draw(_texture, Collected ? new Vector2(-100, -100) : Position, sourceRect, Color.White);
        }

        public void LoadContent(ContentManager content)
        {
            _texture = content.Load<Texture2D>("FlameKey");
        }

        public void Update(GameTime gameTime)
        {
            
        }

        public void UpdateLightMap(LightTileMap tm)
        {
            if (!Collected)
            {
                int X = (int)(TilePosition.X);
                int Y = (int)(TilePosition.Y);

                tm.SetLight(X, Y, 4);
                bool TM = tm.SetLight(X, Y - 1, 4);
                bool TR = tm.SetLight(X + 1, Y - 1, 4);
                bool TL = tm.SetLight(X - 1, Y - 1, 4);
                bool BM = tm.SetLight(X, Y + 1, 4);
                bool BR = tm.SetLight(X + 1, Y + 1, 4);
                bool BL = tm.SetLight(X - 1, Y + 1, 4);
                bool L = tm.SetLight(X - 1, Y, 4);
                bool R = tm.SetLight(X + 1, Y, 4);
                if (!TM) tm.SetLight(X, Y - 2, 4);
                if (!BM) tm.SetLight(X, Y + 2, 4);
                if (!L) tm.SetLight(X - 2, Y, 4);
                if (!R) tm.SetLight(X + 2, Y, 4);
                if (!TR) tm.SetLight(X + 2, Y - 2, 3);
                if (!TL) tm.SetLight(X - 2, Y - 2, 2);
                if (!BR) tm.SetLight(X + 2, Y + 2, 6);
                if (!BL) tm.SetLight(X - 2, Y + 2, 5);
                if (!(TR)) tm.SetLight(X + 1, Y - 2, 4);
                if (!(TL)) tm.SetLight(X - 1, Y - 2, 4);
                if (!(BR)) tm.SetLight(X + 1, Y + 2, 4);
                if (!(BL)) tm.SetLight(X - 1, Y + 2, 4);
                if (!(BL)) tm.SetLight(X - 2, Y + 1, 4);
                if (!(TL)) tm.SetLight(X - 2, Y - 1, 4);
                if (!(BR)) tm.SetLight(X + 2, Y + 1, 4);
                if (!(TR)) tm.SetLight(X + 2, Y - 1, 4);
            }
        }

        public FlameKey(Vector2 pos)
        {
            Position = pos * 64;
        }
    }
}
