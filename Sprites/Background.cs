using CollisionExample.Collisons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonDweller.Sprites
{
    public class Background : ISprite
    {
        //The Torch texture 
        private Texture2D _texture;

        ///<summary>
        /// The Torches position in the world
        ///</summary>
        public Vector2 Position { get; private set; }

        //Not Used
        public BoundingRectangle Bounds { get => new(0,0,0,0);}

        public string Name => "Background";

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position, Color.White);
        }

        public void LoadContent(ContentManager content)
        {
            _texture = content.Load<Texture2D>("Background");
        }

        public void Update(GameTime gameTime)
        {

        }

        public bool Collides(ISprite other)
        {
            return false;
        }

        public Background(Vector2 Pos)
        {
            Position = Pos;
        }
    }
}
