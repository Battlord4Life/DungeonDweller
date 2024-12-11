using CollisionExample.Collisons;
using DungeonDweller.Archetecture;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace DungeonDweller.Sprites
{
    public class LightDweller : ISprite
    {

        //The animated flame texture 
        private Texture2D _texture;


        // The current animation frame 
        private short _animationFrame;

        private Tilemap _tilemap;


        /// <summary>
        /// Scale of the Sprite
        /// </summary>
        private float _scale;

        public bool Horizontal = false;

        public bool LightShine = false;

        public double elapse;
        public double Delapse;

        public Vector2 TargetTile;
        public Vector2 PrevTargetTile;

        public direction Looking = direction.North;

        /// <summary>
        /// 
        /// </summary>
        public float Speed;

        ///<summary>
        /// The flames's position in the world
        ///</summary>
        public Vector2 Position { get; set; }
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
        public BoundingRectangle Bounds { get => new(new(Position.X + 8, Position.Y + 8), 16 * 2, 16 * 2); }

        public string Name => "LightDweller";


        bool _update = false;

        public LightDweller(Vector2 Pos, float speed, Tilemap tim)
        {
            _scale = 4;

            _tilemap = tim;


            Position = Pos * 64;


            Speed = speed;


        }

        public void LoadContent(ContentManager content)
        {
            //Fix with Flame Sprite Sheet
            _texture = content.Load<Texture2D>("LightStalker");
            //else _texture = content.Load<Texture2D>("hflame-spritesheet-32x32");
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

            // Determine the source rectangle 
            var sourceRect = new Rectangle(_animationFrame * 32 * 4, 0, 32 * 4, 32 * 4);

            // Draw the bat using the current animation frame 
            spriteBatch.Draw(_texture, Position + new Vector2(0, -64), sourceRect, Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0);
        }

        public void Update(GameTime gameTime)
        {

            elapse += gameTime.ElapsedGameTime.TotalMilliseconds;

            Delapse += gameTime.ElapsedGameTime.TotalMilliseconds;

            

            if (LightShine)
            {
                if (elapse >= Speed)
                {
                    elapse = 0;
                    Vector2 pos = Findpath();
                     TilePosition = pos;
                }

                Delapse = 0;
            }

            

            PrevTargetTile = TargetTile;

        }

        public Vector2 Findpath()
        {

            if (TargetTile != PrevTargetTile) _tilemap.ResetFindPath(TilePosition, TargetTile);
            else _tilemap.FindPath(TilePosition, TargetTile);
            if (_tilemap.Final.Count > 2)
                return _tilemap.Final[_tilemap.Final.Count - 2];
            else if (_tilemap.Final.Count > 0)
                return _tilemap.Final[0];
            else
                return TilePosition;

        }


        public bool Collides(ISprite other)
        {
            return Bounds.CollidesWith(other.Bounds);
        }

        public void UpdateLightMap(LightTileMap tm)
        {
            int X = (int)(TilePosition.X);
            int Y = (int)(TilePosition.Y);

            if (tm._map[X, Y] == 1) LightShine = false;
            else LightShine = true;


        }
    }
}
