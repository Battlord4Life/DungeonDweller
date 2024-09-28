using CollisionExample.Collisons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using DungeonDweller.Archetecture;

namespace DungeonDweller.Sprites
{

    

    public class Hero : ISprite
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
        /// If the Hero is Moving.
        /// </summary>
        public bool Moving = false;

        /// <summary>
        /// Speed of the sprite
        /// </summary>
        private float _speed = 100;

        private InputManager _inputMan;

        public direction HeroDirection = direction.North;

        ///<summary>
        /// The Heroes's position in the world
        ///</summary>
        public Vector2 Position { get; private set; }

        public BoundingRectangle Bounds => new BoundingRectangle(Position, _scale * 16, _speed * 16);

        public string Name => "Hero";

        private Vector2 _heroVel;

        public Hero(Vector2 Pos, InputManager input)
        {
            Position = Pos;
            _inputMan = input;
            _speed = 100;
            _scale = 4;
            _heroVel = new Vector2(0, 1) * _speed;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //step forward 
            _animationTimer += gameTime.ElapsedGameTime.TotalSeconds;



            if (_animationTimer > (1 / 1.5))
            {
                if (Moving)
                {
                    if (_animationFrame < 2) _animationFrame = 2;
                    _animationFrame++;
                    if (_animationFrame > 5) _animationFrame = 2;
                    
                }
                else
                {
                    if (_animationFrame > 1) _animationFrame = 0;
                    _animationFrame++;
                    if (_animationFrame > 1) _animationFrame = 0;
                }
                _animationTimer -= (1 / 1.5);
            }

            int Box = 16 * (int)HeroDirection * _scale;

            // Determine the source rectangle 
            var sourceRect = new Rectangle(_animationFrame * 16 * _scale, Box, 16 * _scale, 16 * _scale);

            // Draw the bat using the current animation frame 
            spriteBatch.Draw(_texture, Position, sourceRect, Color.White);
        }

        public void LoadContent(ContentManager content)
        {
            _texture = content.Load<Texture2D>("HeroSpriteSheet"); //update with HeroSpriteSheet
        }

        public void Update(GameTime gameTime)
        {
            //_inputMan.Update(gameTime);

            Moving = _inputMan.Moving;

            HeroDirection = _inputMan.EnumDirection;

            Position += _inputMan.Direction;

        }

        public bool Collides(ISprite other)
        {
            return Bounds.CollidesWith(other.Bounds);
        }
    }
}
