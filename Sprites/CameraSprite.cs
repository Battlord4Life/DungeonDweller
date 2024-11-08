﻿using CollisionExample.Collisons;
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
    public class CameraSprite : ISprite
    {
        public Vector2 Position { get; set; }

        public Texture2D _texture;

        public bool Collected = false;

        public BoundingRectangle Bounds => new BoundingRectangle(new(Position.X + 8, Position.Y + 8), 32, 32);

        public string Name => "CameraSprite";

        public bool Collides(ISprite other)
        {
            bool temp = Bounds.CollidesWith(other.Bounds);

            if (temp)
            {
                if(other.Name == "Hero")
                {
                    if (((Hero)other).Items.Contains("Camera"))
                    {
                        ((Hero)other).Bulbs += 2;
                    }
                    else
                    {
                        ((Hero)other).Items.Add("Camera");
                    }
                }
            }
            return temp && !Collected;

        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Collected ? new Vector2(-100,-100) : Position, Color.White);
        }

        public void LoadContent(ContentManager content)
        {
            _texture = content.Load<Texture2D>("Camera64");
        }

        public void Update(GameTime gameTime)
        {
            
        }

        public void UpdateLightMap(LightTileMap tm)
        {
            
        }

        public CameraSprite(Vector2 pos)
        {
            Position = pos;
        }
    }
}