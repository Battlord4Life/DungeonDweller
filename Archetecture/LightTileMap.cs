using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Direct3D9;

namespace DungeonDweller.Archetecture
{
    public class LightTileMap : Tilemap
    {

        Tilemap map;

        float Alpha = 1;

        bool FullAlpha = true;
        public LightTileMap(string fileName, Tilemap tm) : base(fileName)
        {
            map = tm;
        }

       

        public bool SetLight(int x, int y, int light)
        {
            if (x >= _mapWidth || x < 0 || y >= _mapHeight || y < 0) return true;
            _map[x, y] = _map[x,y] == 1 || _map[x, y] == light ? light : 4;
            return map.IsWall(x, y);
            
        }

        public bool SetDark(int x, int y)
        {
            if (x >= _mapWidth || x < 0 || y >= _mapHeight || y < 0) return true;
            _map[x, y] = 1;
            return map.IsWall(x, y);
        }

        public void Reset()
        {
            for(int i = 0; i < _mapWidth; i++)
            {
                for(int j = 0; j< _mapHeight; j++)
                {
                    _map[i, j] = 1;
                }
            }
        }

        public virtual void Update(GameTime gameTime)
        {
            if (!FullAlpha)
            {
                Alpha += .025f;
            }
            if(Alpha >= 1)
            {
                FullAlpha = true;
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for (int y = 0; y < _mapHeight; y++)
            {
                for (int x = 0; x < _mapWidth; x++)
                {
                    int index = _map[x, y] - 1;
                    if (index == -1) continue;
                    spriteBatch.Draw(_tileSetTexture, new Vector2(x * _tileWidth, y * _tileHeight), _tiles[index], FullAlpha ? Color.White : new Color(Color.White, Alpha), 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0);
                }
            }
        }

        public void Flash()
        {
            Alpha = 0;
            FullAlpha = false;
        }

        
    }
}
