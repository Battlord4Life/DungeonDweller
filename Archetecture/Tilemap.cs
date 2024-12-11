using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
//using System.Drawing;
using System.IO;

namespace DungeonDweller.Archetecture
{
    public class Tilemap
    {

        public int _tileWidth, _tileHeight, _mapWidth, _mapHeight;

        protected Texture2D _tileSetTexture;

        protected Rectangle[] _tiles;

        public int[,] _map;

        public int[] WallNodes;

        public int[] HazNodes;

        public bool[,] _wallMap;

        public bool[,] _hazMap;

        public (int g, int h, int f)[,] AStarvals;

        public (int x, int y)[,] Connectons;

        string _filename;

        public List<Vector2> ToSearch;
        public List<Vector2> Searched;
        public List<Vector2> Final;

        public void ResetFindPath(Vector2 Start, Vector2 End)
        {
            AStarvals = new (int, int, int)[_mapWidth, _mapHeight];
            Connectons = new (int, int)[_mapWidth, _mapHeight];
            for (int i = 0; i < _mapHeight; i++)
            {
                for (int j = 0; j < _mapWidth; j++)
                {
                    AStarvals[j, i] = (int.MaxValue, int.MaxValue, int.MaxValue);
                }
            }

            FindPath(Start, End);

        }

        public void FindPath(Vector2 Start, Vector2 End)
        {
            Searched = new();
            ToSearch = new List<Vector2> { Start };
            Final = new();

            AStarvals[(int)Start.X, (int)Start.Y] = (0, GetDistance(Start, End), GetDistance(Start, End));

            while (ToSearch.Count > 0)
            {
                Vector2 celltosearch = ToSearch[0];

                foreach (Vector2 pos in ToSearch)
                {
                    if (AStarvals[(int)pos.X, (int)pos.Y].f < AStarvals[(int)celltosearch.X, (int)celltosearch.Y].f ||
                        AStarvals[(int)pos.X, (int)pos.Y].f == AStarvals[(int)celltosearch.X, (int)celltosearch.Y].f &&
                        AStarvals[(int)pos.X, (int)pos.Y].h == AStarvals[(int)celltosearch.X, (int)celltosearch.Y].h)
                    {
                        celltosearch = pos;
                    }
                }

                ToSearch.Remove(celltosearch);
                Searched.Add(celltosearch);

                if (celltosearch == End)
                {
                    Vector2 curr = End;

                    while (curr != Start)
                    {
                        Final.Add(curr);
                        curr = new Vector2(Connectons[(int)curr.X, (int)curr.Y].x, Connectons[(int)curr.X, (int)curr.Y].y);
                    }

                    Final.Add(Start);

                    return;
                }

                SearchNeighbors(celltosearch, End);
            }
        }

        private void SearchNeighbors(Vector2 cellpos, Vector2 end)
        {
            for (int x = Math.Max((int)cellpos.X - 1, 0); x <= Math.Min((int)cellpos.X + 1, _mapWidth-1); x++)
            {
                for (int y = Math.Max((int)cellpos.Y - 1, 0); y <= Math.Min((int)cellpos.Y + 1, _mapHeight - 1); y++)
                {
                    Vector2 npos = new Vector2(x, y);
                    if (!Searched.Contains(npos) && !_wallMap[x, y])
                    {
                        int neigcost = AStarvals[(int)cellpos.X, (int)cellpos.Y].g + GetDistance(cellpos, npos);

                        if (neigcost < AStarvals[x, y].g)
                        {
                            Connectons[x, y] = ((int)cellpos.X, (int)cellpos.Y);
                            AStarvals[x, y].g = neigcost;
                            AStarvals[x, y].h = GetDistance(npos, end);
                            AStarvals[x, y].f = AStarvals[x, y].g + AStarvals[x, y].h;

                            if (!ToSearch.Contains(npos))
                            {
                                ToSearch.Add(npos);
                            }
                        }
                    }
                }
            }
        }

        private int GetDistance(Vector2 pos1, Vector2 pos2)
        {
            Vector2 dist = new Vector2(MathF.Abs(pos1.X - pos2.X), MathF.Abs(pos1.Y - pos2.Y));

            int Highest = (int)MathF.Max(dist.X, dist.Y);
            int lowest = (int)MathF.Min(dist.X, dist.Y);

            int hori = Highest - lowest;

            return lowest * 14 + hori * 10;
        }

        public Tilemap(string fileName)
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

            for (int y = 0; y < tileColums; y++)
            {
                for (int x = 0; x < tileRows; x++)
                {
                    int index = y * tileColums + x;
                    _tiles[index] = new Rectangle(
                        x * _tileWidth,
                        y * _tileHeight,
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
            foreach (var thin in fourthLine)
            {
                int temp = int.Parse(thin);
                if (temp != 0)
                {
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
            AStarvals = new (int, int, int)[_mapWidth, _mapHeight];
            Connectons = new (int, int)[_mapWidth, _mapHeight];
            for (int i = 0; i < _mapHeight; i++)
            {
                var fourth = line[i + 5].Split(',');
                for (int j = 0; j < _mapWidth; j++)
                {
                    int temp = int.Parse(fourth[j]);
                    _map[j, i] = temp;

                    bool haz = false;
                    foreach (int k in HazNodes)
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

                    AStarvals[j, i] = (int.MaxValue, int.MaxValue, int.MaxValue);
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

        public IEnumerable<((int, int), direction)> ValidMoves(int x, int y)
        {
            if (!IsWall(x, y - 1) && !IsHaz(x, y - 1)) yield return ((x, y - 1), direction.North);
            if (!IsWall(x + 1, y) && !IsHaz(x + 1, y)) yield return ((x + 1, y), direction.East);
            if (!IsWall(x, y + 1) && !IsHaz(x, y + 1)) yield return ((x, y + 1), direction.South);
            if (!IsWall(x - 1, y) && !IsHaz(x - 1, y)) yield return ((x - 1, y), direction.West);
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
                    int index = _map[x, y] - 1;
                    if (index == -1) continue;
                    spriteBatch.Draw(_tileSetTexture, new Vector2(x * _tileWidth, y * _tileHeight), _tiles[index], Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0);
                }
            }
        }
    }
}
