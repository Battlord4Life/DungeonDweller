using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CollisionExample.Collisons;
using DungeonDweller.Archetecture;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct3D9;

namespace DungeonDweller.Sprites
{
    public class Flame: ISprite
    {

        //The animated flame texture 
        private Texture2D _texture;

        // A timer variable for sprite animation
        private double _animationTimer;

        // The current animation frame 
        private short _animationFrame;

        /// <summary>
        /// Scale of the Sprite
        /// </summary>
        private int _scale;

        /// <summary>
        /// 
        /// </summary>
        private float _speed;

        ///<summary>
        /// The flames's position in the world
        ///</summary>
        public Vector2 Position { get; private set; }
        public BoundingRectangle Bounds { get => new(new(Position.X + 16, Position.Y + 16), 16 * _scale, 16 * _scale);}

        public string Name => "Flame";

        private float _maxHeight;

        private float _minHeight;

        private Vector2 _falmeVel;

        private bool _hori;

        private Vector2 Pointer = new(0,0);

        bool _update = false;

        public Flame(Vector2 Pos, float Diff, bool flip)
        {
            _scale = 4;
            Position = Pos;
            _maxHeight = Pos.Y - Diff;
            _minHeight = Pos.Y + Diff;
            _speed = 100;
            _hori = flip;
            _falmeVel = new Vector2(0, 1) * _speed;

        }

        public void LoadContent(ContentManager content)
        {
            //Fix with Flame Sprite Sheet
           if(!_hori) _texture = content.Load<Texture2D>("flame-spritesheet-32x32");
           else _texture = content.Load<Texture2D>("hflame-spritesheet-32x32");
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //step forward 
            _animationTimer += gameTime.ElapsedGameTime.TotalSeconds;

           
            if (_animationTimer > (1/1.5))
            {
                _animationFrame++;
                if (_animationFrame > 7) _animationFrame = 0;
                _animationTimer -= (1/1.5);
            }

            // Determine the source rectangle 
            var sourceRect = new Rectangle(_animationFrame * 32 * _scale, 0, 32 * _scale, 32* _scale);
            
            // Draw the bat using the current animation frame 
            spriteBatch.Draw(_texture, Position, sourceRect, Color.White);
        }

        public void Update(GameTime gameTime)
        {
            
            
            Pointer += _falmeVel * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if(Pointer.X >= 64)
            {
                Position = new(Position.X + 64, Position.Y);
                Pointer = new(0, Pointer.Y);
                _update = false;
            }
            if (Pointer.X <= -64)
            {
                Position = new(Position.X - 64, Position.Y);
                Pointer = new(0, Pointer.Y);
                _update = false;
            }
            if (Pointer.Y >= 64)
            {
                Position = new(Position.X , Position.Y + 64);
                Pointer = new(Pointer.X, 0);
                _update = false;
            }
            if (Pointer.Y <= -64)
            {
                Position = new(Position.X, Position.Y - 64);
                Pointer = new(Pointer.X, 0);
                _update = false;
            }
            if ((Position.Y >= _minHeight || Position.Y <= _maxHeight) && !_update) 
            { 
                _falmeVel.Y *= -1;
                _update = true;
            }
            
        }

        public bool Collides(ISprite other)
        {
            return Bounds.CollidesWith(other.Bounds);
        }

        public void UpdateLightMap(LightTileMap tm)
        {
            int X = (int)(Position.X / 64);
            int Y = (int)(Position.Y / 64);


            tm.SetLight(X, Y, 4);
            tm.SetLight(X+1, Y, 4);
            tm.SetLight(X, Y+1, 4);
            tm.SetLight(X+1, Y+1, 4);
            
        }
    }
}
