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

        public int SelectedItem = 1;

        public List<string> Items = new List<string>();

        ///<summary>
        /// The Heroes's position in the world
        ///</summary>
        public Vector2 Position { get; private set; }

        public BoundingRectangle Bounds => new BoundingRectangle(new(Position.X + 8, Position.Y + 8), _scale * 8, _scale * 8);

        public string Name => "Hero";

        private Vector2 _heroVel;

        private Tilemap _map;

        public Hero(Vector2 Pos, InputManager input)
        {
            Position = Pos;
            _inputMan = input;
            _speed = 100;
            _scale = 4;
            _heroVel = new Vector2(0, 1) * _speed;
            Items.Add("None");
            //Items.Add("Flash");
            
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

        public void UpdateTile(Tilemap tm)
        {
            _map = tm;
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

            Vector2 XY = Position / _map._tileHeight;

            if (_inputMan.MoveUp)
            {
                
                if(!_map.IsWall((int)XY.X, (int)XY.Y - 1)) Position = new(Position.X , Position.Y - 64f);
            }
            if (_inputMan.MoveLeft)
            {
                if (!_map.IsWall((int)XY.X-1, (int)XY.Y)) Position = new(Position.X - 64f, Position.Y);
            }
            if (_inputMan.MoveRight)
            {
                if (!_map.IsWall((int)XY.X +1, (int)XY.Y)) Position = new(Position.X + 64f, Position.Y);
            }
            if (_inputMan.MoveDown)
            {
                if (!_map.IsWall((int)XY.X, (int)XY.Y + 1)) Position = new(Position.X, Position.Y + 64f);
            }
            if(_inputMan.Switch0 && Items.Contains("None"))
            {
                SelectedItem = 0;
            }
            if (_inputMan.Switch1 && Items.Contains("Flash"))
            {
                SelectedItem = 1;
            }
            if (_inputMan.Switch2 && Items.Contains("Lant"))
            {
                SelectedItem = 2;
            }
            if (_inputMan.Switch3 && Items.Contains("Cam"))
            {
                SelectedItem = 3;
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


            if (SelectedItem == 1)
            {
                switch (HeroDirection)
                {
                    case direction.North:
                        tm.SetLight(X, Y, 4);
                        bool firstN = tm.SetLight(X, Y - 1, 4);
                        bool secondLN = tm.SetLight(X - 1, Y - 1, 5);
                        bool secondRN = tm.SetLight(X + 1, Y - 1, 6);

                        

                        if (!firstN) if(!tm.SetLight(X, Y - 2, 4)) tm.SetLight(X, Y-3, 4);
                        if (!(secondLN)) if(!tm.SetLight(X - 1, Y - 2, 4)) tm.SetLight(X - 1, Y - 3, 2); ;
                        if (!(secondRN)) if(!tm.SetLight(X + 1, Y - 2, 4)) tm.SetLight(X + 1, Y - 3, 3);

                        break;
                    case direction.South:


                        tm.SetLight(X, Y, 4);
                        bool firstS = tm.SetLight(X, Y + 1, 4);
                        bool secondLS = tm.SetLight(X - 1, Y + 1, 2);
                        bool secondRS = tm.SetLight(X + 1, Y + 1, 3);


                        if (!firstS) if(!tm.SetLight(X, Y + 2, 4)) tm.SetLight(X, Y + 3, 4);
                        if (!(secondLS)) if(!tm.SetLight(X - 1, Y + 2, 4)) tm.SetLight(X - 1, Y + 3, 5); ;
                        if (!(secondRS)) if(!tm.SetLight(X + 1, Y + 2, 4)) tm.SetLight(X + 1, Y + 3, 6); ;


                        break;
                    case direction.East:


                        tm.SetLight(X, Y, 4);
                        bool firstE = tm.SetLight(X + 1, Y, 4);
                        bool secondLE = tm.SetLight(X + 1, Y + 1, 5);
                        bool secondRE = tm.SetLight(X + 1, Y - 1, 2);


                        if (!firstE) if(!tm.SetLight(X + 2, Y, 4)) tm.SetLight(X + 3, Y, 4); ;
                        if (!(secondLE)) if(!tm.SetLight(X + 2, Y + 1, 4)) tm.SetLight(X + 3, Y + 1, 6);
                        if (!(secondRE)) if(!tm.SetLight(X + 2, Y - 1, 4)) tm.SetLight(X + 3, Y - 1, 3);



                        break;
                    case direction.West:


                        tm.SetLight(X, Y, 4);
                        bool firstW = tm.SetLight(X - 1, Y, 4);
                        bool secondLW = tm.SetLight(X - 1, Y + 1, 6);
                        bool secondRW = tm.SetLight(X - 1, Y - 1, 3);


                        if (!firstW) if(!tm.SetLight(X - 2, Y, 4)) tm.SetLight(X - 3, Y, 4);
                        if (!(secondLW)) if(!tm.SetLight(X - 2, Y + 1, 4)) tm.SetLight(X - 3, Y + 1, 5);
                        if (!(secondRW)) if(!tm.SetLight(X - 2, Y - 1, 4)) tm.SetLight(X - 3, Y - 1, 2);

                        break;
                }
            }
            if(SelectedItem == 2)
            {
                tm.SetLight(X, Y, 4);
                bool TM = tm.SetLight(X, Y - 1, 4);
                bool TR = tm.SetLight(X+1, Y - 1, 4);
                bool TL = tm.SetLight(X-1, Y - 1, 4);
                bool BM = tm.SetLight(X, Y + 1, 4);
                bool BR = tm.SetLight(X+1, Y + 1, 4);
                bool BL = tm.SetLight(X-1, Y + 1, 4);
                bool L = tm.SetLight(X - 1, Y, 4);
                bool R = tm.SetLight(X + 1, Y, 4);
                if(!TM) tm.SetLight(X, Y - 2, 4);
                if (!BM) tm.SetLight(X, Y + 2, 4);
                if (!L) tm.SetLight(X-2, Y, 4);
                if (!R) tm.SetLight(X+2, Y, 4);
                if(!TR) tm.SetLight(X + 2, Y - 2, 3);
                if(!TL) tm.SetLight(X - 2, Y - 2, 2);
                if (!BR) tm.SetLight(X + 2, Y + 2, 6);
                if (!BL) tm.SetLight(X - 2, Y + 2, 5);
                if (!(TR)) tm.SetLight(X + 1, Y - 2, 4);
                if (!(TL)) tm.SetLight(X - 1, Y - 2, 4);
                if (!(BR)) tm.SetLight(X + 1, Y + 2, 4);
                if ( !(BL)) tm.SetLight(X - 1, Y + 2, 4);
                if (!(BL)) tm.SetLight(X - 2, Y + 1, 4);
                if (!(TL)) tm.SetLight(X - 2, Y - 1, 4);
                if (!(BR)) tm.SetLight(X + 2, Y + 1, 4);
                if (!(TR)) tm.SetLight(X + 2, Y - 1, 4);

            }
            if(SelectedItem == 3)
            {
                if (_inputMan.Flash)
                {
                    tm.Flash();
                }
            }

        }

    }
}
