﻿using CollisionExample.Collisons;
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
    public class Torch : ISprite
    {
        //The Torch texture 
        private Texture2D _texture;

        public bool Active = false;

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

        public BoundingRectangle Bounds => new BoundingRectangle(new(Position.X + 8, Position.Y + 8), 32, 32);

        public string Name => "Torch";

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position + new Vector2(16, 0), Color.White);
        }

        public void LoadContent(ContentManager content)
        {
            _texture = content.Load<Texture2D>("TorchSprite");
        }

        public void Update(GameTime gameTime)
        {

        }

        public bool Collides(ISprite other)
        {
            bool temp = Bounds.CollidesWith(other.Bounds);

            if (temp)
            {
                if (other.Name == "Hero")
                {
                    if (((Hero)other).ToolActive)
                    {
                        Active = true;
                    }
                    
                }
            }
            return temp;
        }

        public void UpdateLightMap(LightTileMap tm)
        {
            int X = (int)(TilePosition.X);
            int Y = (int)(TilePosition.Y);

            if (Active)
            {
                tm.SetLight(X, Y, 4);
                bool TM = tm.SetLight(X, Y - 1, 4);
                bool TR = tm.SetLight(X + 1, Y - 1, 4);
                bool TL = tm.SetLight(X - 1, Y - 1, 4);
                bool BM = tm.SetLight(X, Y + 1, 4);
                bool BR = tm.SetLight(X + 1, Y + 1, 4);
                bool BL = tm.SetLight(X - 1, Y + 1, 4);
                bool L = tm.SetLight(X - 1, Y, 4);
                bool R = tm.SetLight(X + 1, Y, 4);
                if (!TM) tm.SetLight(X, Y - 2, 4);
                if (!BM) tm.SetLight(X, Y + 2, 4);
                if (!L) tm.SetLight(X - 2, Y, 4);
                if (!R) tm.SetLight(X + 2, Y, 4);
                if (!TR) tm.SetLight(X + 2, Y - 2, 3);
                if (!TL) tm.SetLight(X - 2, Y - 2, 2);
                if (!BR) tm.SetLight(X + 2, Y + 2, 6);
                if (!BL) tm.SetLight(X - 2, Y + 2, 5);
                if (!(TR)) tm.SetLight(X + 1, Y - 2, 4);
                if (!(TL)) tm.SetLight(X - 1, Y - 2, 4);
                if (!(BR)) tm.SetLight(X + 1, Y + 2, 4);
                if (!(BL)) tm.SetLight(X - 1, Y + 2, 4);
                if (!(BL)) tm.SetLight(X - 2, Y + 1, 4);
                if (!(TL)) tm.SetLight(X - 2, Y - 1, 4);
                if (!(BR)) tm.SetLight(X + 2, Y + 1, 4);
                if (!(TR)) tm.SetLight(X + 2, Y - 1, 4);
            }
        }

        public Torch(Vector2 Pos)
        {
            Position = Pos *64;
        }
    }
}
