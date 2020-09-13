using System;
using System.Linq;
using System.Text;

public class Map
{
    public int Height { get; }
    public int Width { get; }
    private Node[] _tiles;

    public Map(int width, int height)
    {
        Height = height;
        Width = width;
        _tiles = new Node[Width * Height];
        var random = new Random();
        for (int y = 0; y < Height; y++)
            for (int x = 0; x < Width; x++)
            {
                _tiles[y * Width + x] = new Node
                {
                    Position = new Position { X = x, Y = y},
                    Obstacle = random.NextDouble() <= .20f,
                    Visited = false
                };

            }

        _tiles[0].Obstacle = false;
    }

    public Node GetNode(Position position)
    {
        return _tiles[position.Y * Width + position.X];
    }

    public override string ToString()
    {
        var map = new StringBuilder();
        for (int x = 0; x < Width; x++)
        {
            map.Append('[');
            for (int y = 0; y < Height; y++)
            {
                var tile = _tiles[y*Width+x];
                map.Append($"{(tile.Obstacle ? 'X' : ' ')}");
            }
            map.Append("]\n");
        }
        return map.ToString();
    }

    public string DrawRoute(Gene gene)
    {
        gene.Route.Add(new Node { Position = new Position { X = 0, Y = 0 }, Visited = true });
        var map = new StringBuilder();
        for (int x = 0; x < Width; x++)
        {
            map.Append('[');
            for (int y = 0; y < Height; y++)
            {
                var tile = _tiles[y*Width+x];
                var route = gene.Route.FirstOrDefault(move => move.Position.X == x && move.Position.Y == y);
                if ( tile.Obstacle )
                    map.Append('X');
                else if ( route != null )
                    map.Append('*');
                else
                    map.Append(' ');
            }
            map.Append("]\n");
        }
        return map.ToString();
    }
}
