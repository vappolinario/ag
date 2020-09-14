using System;
using System.Linq;
using System.Text;

namespace AgPath
{
    public class Map
    {
        public int Height { get; }
        public int Width { get; }
        private Node[] _tiles;

        private const char EMPTY = ' ';
        private const char OBSTACLE = 'X';
        private const char ROUTE = '*';

        public Map(int width, int height, double obstacleRate = .2f)
        {
            Height = height;
            Width = width;
            _tiles = new Node[Width * Height];
            CreateObstacles(obstacleRate);
            _tiles[0].Obstacle = false;
        }

        private void CreateObstacles(double rate)
        {
            var random = new Random();
            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                {
                    _tiles[y * Width + x] = new Node
                    {
                        Position = new Position { X = x, Y = y },
                        Obstacle = random.NextDouble() <= rate,
                        Visited = false
                    };
                }
        }

        public Node GetNode(Position position)
        {
            return _tiles[position.Y * Width + position.X];
        }

        public override string ToString()
        {
            return DrawRoute(new Gene(2, null));
        }

        public string DrawRoute(Gene gene)
        {
            var startingPosition = new Position { X = 0, Y = 0 };
            gene.Route.Add(new Node
            {
                Position = startingPosition,
                Visited = true
            });
            var map = new StringBuilder();
            map.Append(new String('-', Width + 2));
            map.Append('\n');
            for (int x = 0; x < Width; x++)
            {
                map.Append('[');
                for (int y = 0; y < Height; y++)
                {
                    var tile = _tiles[y * Width + x];
                    var route = gene.Route.FirstOrDefault(move => move.Position.X == x && move.Position.Y == y);
                    if (tile.Obstacle)
                        map.Append(OBSTACLE);
                    else if (route != null)
                        map.Append(ROUTE);
                    else
                        map.Append(EMPTY);
                }
                map.Append("]\n");
            }
            map.Append(new String('-', Width + 2));
            return map.ToString();
        }
    }
}
