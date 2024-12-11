using CollisionExample.Collisons;
using DungeonDweller.Archetecture;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonDweller.Sprites
{
    public class FireOrb : ISprite
    {

        //The Torch texture 
        private Texture2D _texture;
        private Texture2D _Opentexture;



        public bool Open = false;
        private double _animationTimer;
        private int _animationFrame;

        ///<summary>
        /// The Torches position in the world
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

        public BoundingRectangle Bounds => new BoundingRectangle(new(Position.X + 16, Position.Y + 16), 64, 64);

        public string Name => "FireOrb";

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

            //step forward 
            _animationTimer += gameTime.ElapsedGameTime.TotalSeconds;


            if (_animationTimer > (1 / 1.99))
            {
                _animationFrame++;
                if (_animationFrame > 3) _animationFrame = 0;
                _animationTimer -= (1 / 1.99);
            }

            var sourceRect = new Rectangle(_animationFrame * 32 * 4, 0, 32 * 4, 32 * 4);

            // Draw the bat using the current animation frame 
           
            if (Open)
            {
                spriteBatch.Draw(_Opentexture, Position + new Vector2(0, 0), sourceRect, Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0);

            }
            else
            {
                spriteBatch.Draw(_texture, Position + new Vector2(0, 0), sourceRect, Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0);


            }
        }

        public void LoadContent(ContentManager content)
        {
            _texture = content.Load<Texture2D>("ClosedFireOrb");
            _Opentexture = content.Load<Texture2D>("UnlockedOrb");
        }

        public void Update(GameTime gameTime)
        {

        }

        public bool Collides(ISprite other)
        {
            bool temp = Bounds.CollidesWith(other.Bounds);

           
            
            return temp;
        }

        public void UpdateLightMap(LightTileMap tm)
        {
            
        }

        public FireOrb(Vector2 Pos)
        {
            Position = Pos *64;
        }
    }
}
