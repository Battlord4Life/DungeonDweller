using CollisionExample.Collisons;
using DungeonDweller.Archetecture;
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
    public class _3DCube : ISprite
    {
        public BoundingRectangle Bounds => new BoundingRectangle(0, 0, 0, 0);

        public string Name => "Cube";

        public Vector2 Position { get; set ; }

        public bool Collides(ISprite other)
        {
            return false;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            
        }

        public void LoadContent(ContentManager content)
        {
            
        }

        public void Update(GameTime gameTime)
        {
            
        }

        public void UpdateLightMap(LightTileMap tm)
        {
            
        }
    }
}
