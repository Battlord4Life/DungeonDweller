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
    public class ExitSprite : ISprite
    {

        //The Torch texture 
        private Texture2D _texture;

        public bool Open = false;

        ///<summary>
        /// The Torches position in the world
        ///</summary>
        public Vector2 Position { get; set; }

        public BoundingRectangle Bounds => new BoundingRectangle(new(Position.X + 8, Position.Y + 8), 32, 32);

        public string Name => "Exit";

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (Open)
            {
                spriteBatch.Draw(_texture, Position,new Rectangle(0,0, 64, 64), Color.White);
            }
            else
            {
                spriteBatch.Draw(_texture, Position, new Rectangle(64, 0, 64, 64), Color.White);

            }
        }

        public void LoadContent(ContentManager content)
        {
            _texture = content.Load<Texture2D>("ExitSheet");
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

        public ExitSprite(Vector2 Pos)
        {
            Position = Pos;
        }
    }
}
