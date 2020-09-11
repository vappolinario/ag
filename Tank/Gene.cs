using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

public class Gene : IComparable<Gene>, IEquatable<Gene>
{
    public double Fitness { get; set; }
    public List<int> Chromosomes { get; set; }
    public Position Position { get; set; }
    public List<Position> Route { get; set; }

    public Gene(int size = 40)
    {
        var random = new Random();
        Chromosomes = Enumerable
            .Range(0, size)
            .Select(_ => random.Next(0,2))
            .ToList();
        Fitness = 0;
        Route = new List<Position>();
    }

    internal void IterateRoute(Position start, Position destiny)
    {
        Position = start;
        Route.Clear();
        Fitness = 0;
        for (int index = 0; index < Chromosomes.Count(); index+=2)
        {
            if ( !Position.Equals(destiny) )
            {
                Move(index, destiny);
                Route.Add(Position);
            }
            Fitness += ComputeFitness(destiny);
        }
    }

    private double ComputeFitness(Position destiny)
    {
        var distance = Math.Pow(destiny.X - Position.X, 2);
        distance += Math.Pow(destiny.Y - Position.Y, 2);
        distance = Math.Sqrt(distance);
        return 1f/(distance + Route.Count());
    }

    private bool Move(int index, Position destiny)
    {
        int x = Position.X;
        int y = Position.Y;
        var movement = string.Join("", Chromosomes.GetRange(index, 2));
        switch (movement)
        {
            case "00":
                x++;
                break;
            case "01":
                y++;
                break;
            case "10":
                x--;
                break;
            case "11":
                y--;
                break;
        }
        var temp = new Position { X = x, Y = y};
        if (!ValidatePosition(temp))
        {
            return false;
        }
        Position = temp;
        return true;
    }

    private bool ValidatePosition(Position position)
    {
        if ( position.X < 0 || position.X > 9 )
            return false;

        if (position.Y < 0 || position.Y > 9 )
            return false;

        return true;
    }

    public void Mutate(int threshold)
    {
        Chromosomes = Chromosomes.Select(i => MutateBit(i, threshold)).ToList();
    }

    private int MutateBit(int chromosome, int threshold)
    {
        var random = new Random();
        var dice = random.Next(0, 1000);
        return dice <= threshold ? 1 - chromosome : chromosome;
    }

    public int CompareTo([AllowNull] Gene other)
    {
        if (other == null) return -1;
        return other.Fitness.CompareTo(this.Fitness);
    }

    public bool Equals([AllowNull] Gene other)
    {
        if (other == null) return false;
        return string.Join("", Chromosomes)
            .Equals(string.Join("", other.Chromosomes));
    }

    public override string ToString()
    {
        return $"[{Fitness:F4}] - [{Route.Count()}] - [{string.Join(";", Route)}]";
    }
}

