using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
//using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace DungeonDweller.Archetecture
{
    public class CusTilemap
    {

        public int _tileWidth, _tileHeight, _mapWidth, _mapHeight;

        protected Texture2D _tileSetTexture;

        protected Rectangle[] _tiles;

        public int[,] _map;

        public int[] WallNodes;

        public int[] HazNodes;

        public bool[,] _wallMap;

        public bool[,] _hazMap;

        string _filename;

        public CusTilemap(string fileName)
        {
            _filename = fileName;
        }

        public void LoadContent(ContentManager content)
        {
            string data = File.ReadAllText(Path.Join(content.RootDirectory, _filename));
            var line = data.Split('\n');

            var tilesetFilename = line[0].Trim();
            _tileSetTexture = content.Load<Texture2D>(tilesetFilename);

            var secondLine = line[1].Split(',');
            _tileWidth = int.Parse(secondLine[0]);
            _tileHeight = int.Parse(secondLine[1]);

            int tileColums = _tileSetTexture.Width / _tileWidth;

            int tileRows = _tileSetTexture.Height / _tileHeight;

            _tiles = new Rectangle[tileColums * tileRows];

            for(int y = 0; y < tileColums; y++)
            {
                for (int x = 0; x < tileRows; x++)
                {
                    int index = y * tileColums + x;
                    _tiles[index] = new Rectangle(
                        x*_tileWidth,
                        y*_tileHeight,
                        _tileWidth,
                        _tileHeight
                        );
                }
            }

            var thirdLine = line[2].Split(',');
            _mapWidth = int.Parse(thirdLine[0]);
            _mapHeight = int.Parse(thirdLine[1]);

            var fourthLine = line[3].Split(',');
            int tem = 0;
            WallNodes = new int[fourthLine.Length];
            foreach(var thin in fourthLine)
            {
                int temp = int.Parse(thin);
                if (temp != 0) {
                    WallNodes[tem] = temp;
                    tem++;
                }
            }

            var fifthLine = line[4].Split(',');
            tem = 0;
            HazNodes = new int[fifthLine.Length];
            foreach (var thin in fifthLine)
            {
                int temp = int.Parse(thin);
                if (temp != 0)
                {
                    HazNodes[tem] = temp;
                    tem++;
                }
            }



            _map = new int[_mapWidth, _mapHeight];
            _wallMap = new bool[_mapWidth, _mapHeight];
            _hazMap = new bool[_mapWidth, _mapHeight];
            for (int i = 0; i < _mapHeight; i++)
            {
                var fourth = line[i+5].Split(',');
                for(int j = 0; j < _mapWidth; j++)
                {
                    int temp = int.Parse(fourth[j]);
                    _map[j,i] = temp;

                    bool haz = false;
                    foreach(int k in HazNodes)
                    {
                        if (temp == k) haz = true;
                    }
                    _hazMap[j, i] = haz;

                    bool wall = false;
                    foreach (int k in WallNodes)
                    {
                        if (temp == k) wall = true;
                    }
                    _wallMap[j, i] = wall;
                }
            }

            
        }

        public int GetCell(int x, int y)
        {
            if (x >= _mapWidth || x < 0 || y >= _mapHeight || y < 0) return 0;
            return _map[x, y];
        }

        public bool IsWall(int x, int y)
        {
            if (x >= _mapWidth || x < 0 || y >= _mapHeight || y < 0) return true;
            return _wallMap[x, y];
        }

        public bool IsHaz(int x, int y)
        {
            if (x >= _mapWidth || x < 0 || y >= _mapHeight || y < 0) return false;
            return _hazMap[x, y];
        }

        public void SetCell(int x, int y, int change)
        {
            if (x >= _mapWidth || x < 0 || y >= _mapHeight || y < 0) return;

            _map[x, y] = change;
            
            bool haz = false;
            foreach (int k in HazNodes)
            {
                if (change == k) haz = true;
            }
            _hazMap[x, y] = haz;
            bool wall = false;
            foreach (int k in WallNodes)
            {
                if (change == k) wall = true;
            }
            _wallMap[x, y] = wall;
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for (int y = 0; y < _mapHeight; y++)
            {
                for (int x = 0; x < _mapWidth; x++)
                {
                    int index = _map[x,y] - 1;
                    if (index == -1) continue;
                    spriteBatch.Draw(_tileSetTexture, new Vector2(x * _tileWidth, y * _tileHeight), _tiles[index], Color.White, 0f,new Vector2(0,0), 1f, SpriteEffects.None, 0);
                }
            }
        }
    }
}
