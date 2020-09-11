using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

public class Gene : IComparable<Gene>, IEquatable<Gene>
{
    public double Fitness { get; set; }
    public BitArray Chromosomes { get; set; }
    public Position Position { get; set; }
    public List<Position> Route { get; set; }

    public Gene(int size = 40)
    {
        var random = new Random();
        Chromosomes = new BitArray(size);
        for (int index = 0; index < Chromosomes.Length; index++)
        {
            Chromosomes.Set(index, random.NextDouble() >= 0.5);
        }
        Fitness = 0;
        Route = new List<Position>();
    }

    internal void IterateRoute(Position start, Position destiny)
    {
        Position = start;
        Route.Clear();
        Fitness = 0;
        for (int index = 0; index < Chromosomes.Length; index+=2)
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
        var movement =
            $"{(Chromosomes[index] ? 1 : 0)}{(Chromosomes[index+1] ? 1 : 0)}";
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
            default:
                throw new InvalidOperationException($"Invalid movement {movement}");
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
        for (int index = 0; index < Chromosomes.Length; index++)
        {
           MutateBit(index, threshold);
        }
    }

    private void MutateBit(int chromosome, int threshold)
    {
        var random = new Random();
        var dice = random.Next(0, 1000);
        if ( dice <= threshold )
        {
            Chromosomes.Set(chromosome, !Chromosomes.Get(chromosome));
        }
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

