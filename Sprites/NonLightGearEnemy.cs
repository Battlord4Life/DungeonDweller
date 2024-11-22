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
    public class NonLightGearEnemy: ISprite
    {

        //The animated flame texture 
        private Texture2D _texture;

        // A timer variable for sprite animation
        private double _animationTimer;

        // The current animation frame 
        private short _animationFrame;

        private Texture2D _trackTexture;


        /// <summary>
        /// Scale of the Sprite
        /// </summary>
        private float _scale;

        public bool Horizontal = false;

        /// <summary>
        /// 
        /// </summary>
        public float Speed;

        ///<summary>
        /// The flames's position in the world
        ///</summary>
        public Vector2 Position { get;  set; }
        public Vector2 TilePosition { get
            {

                return Position / 64;
            }
            set {


                Position = value * 64;

            } }
        public BoundingRectangle Bounds { get => new(new(Position.X + 16, Position.Y + 16), 16 * _scale, 16 * _scale);}

        public string Name => "Gear";

        private float _maxDist;

        private float _minDist;

        private Vector2 _falmeVel;

        private bool _hori;

        private Vector2 Pointer = new(0,0);

        bool _update = false;

        public NonLightGearEnemy(Vector2 Pos, int Diff, bool horizontal, float speed)
        {
            _scale = 4;


            Position = Pos * 64;

            Horizontal = horizontal;
            if (Horizontal)
            {
                _maxDist = Pos.X + Diff;
                _minDist = Pos.X - Diff;
                _falmeVel = new Vector2(1, 0) * speed;

            }
            else
            {
                _maxDist = Pos.Y - Diff;
                _minDist = Pos.Y + Diff;
                _falmeVel = new Vector2(0, 1) * speed;
            }

            
            Speed = speed;
            
            
        }

        public void LoadContent(ContentManager content)
        {
            //Fix with Flame Sprite Sheet
           _texture = content.Load<Texture2D>("NonFlameFinalGear");
           _trackTexture = content.Load<Texture2D>("GearTrack");
           //else _texture = content.Load<Texture2D>("hflame-spritesheet-32x32");
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //step forward 
            _animationTimer += gameTime.ElapsedGameTime.TotalSeconds;

           
            if (_animationTimer > (1/1.99))
            {
                _animationFrame++;
                if (_animationFrame > 7) _animationFrame = 0;
                _animationTimer -= (1/1.99);
            }


            for(int i = Horizontal ? (int)_minDist : (int)_maxDist; i <=  (Horizontal? (int)_maxDist : (int)_minDist) +1; i++)
            {
                Vector2 temp;
                float rot;
                if (!Horizontal) { temp = new Vector2(Position.X + 32, i * 64); rot = 0; }
                else { temp = new Vector2(i * 64, Position.Y + 32); rot = (float)Math.PI / 2; }

                spriteBatch.Draw(_trackTexture, temp, new Rectangle(0, 0, 64, 64), Color.White, rot, new Vector2(0, 0), 1f, SpriteEffects.None, 0);

            }

            // Determine the source rectangle 
            var sourceRect = new Rectangle(_animationFrame * 32 * 4, 0, 32 * 4, 32* 4);
            
            // Draw the bat using the current animation frame 
            spriteBatch.Draw(_texture, Position+new Vector2(0,0), sourceRect, Color.White, 0f, new Vector2(0,0), 1f, SpriteEffects.None, 0);
        }

        public void Update(GameTime gameTime)
        {
            
            
            Pointer += _falmeVel * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if(Pointer.X >= 64)
            {
                TilePosition = new(TilePosition.X + 1, TilePosition.Y);
                Pointer = new(0, Pointer.Y);
                _update = false;
            }
            if (Pointer.X <= -64)
            {
                TilePosition = new(TilePosition.X - 1, TilePosition.Y);
                Pointer = new(0, Pointer.Y);
                _update = false;
            }
            if (Pointer.Y >= 64)
            {
                TilePosition = new(TilePosition.X , TilePosition.Y + 1);
                Pointer = new(Pointer.X, 0);
                _update = false;
            }
            if (Pointer.Y <= -64)
            {
                TilePosition = new(TilePosition.X, TilePosition.Y - 1);
                Pointer = new(Pointer.X, 0);
                _update = false;
            }
            if (!Horizontal && (TilePosition.Y >= _minDist || TilePosition.Y <= _maxDist) && !_update) 
            { 
                _falmeVel.Y *= -1;
                _update = true;
            }
            if (Horizontal && (TilePosition.X <= _minDist || TilePosition.X >= _maxDist) && !_update)
            {
                _falmeVel.X *= -1;
                _update = true;
            }

        }

        public bool Collides(ISprite other)
        {
            return Bounds.CollidesWith(other.Bounds);
        }

        public void UpdateLightMap(LightTileMap tm)
        {
            
            
        }
    }
}
