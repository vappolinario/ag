using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using AgPath.Fitness;

namespace AgPath
{
    public class Gene : IComparable<Gene>, IEquatable<Gene>
    {
        public double Fitness { get; set; }
        public BitArray Chromosomes { get; set; }
        public Position Position { get; set; }
        public List<Node> Route { get; set; }

        private readonly IFitnessEngine _engine;

        public Gene(int size, IFitnessEngine fitnessEngine)
        {
            _engine = fitnessEngine;
            var random = new Random();
            Chromosomes = new BitArray(size);
            for (int index = 0; index < Chromosomes.Length; index++)
            {
                Chromosomes.Set(index, random.NextDouble() >= 0.5);
            }
            Fitness = 0;
            Route = new List<Node>();
        }

        internal void IterateRoute(Position start, Position destiny, Map map)
        {
            Position = start;
            Route.Clear();
            Fitness = 0;
            for (int index = 0; index < Chromosomes.Length; index+=2)
            {
                if ( !Position.Equals(destiny) )
                {
                    Move(index, destiny, map);
                    Route.Add(new Node {
                            Position = Position,
                            Visited = true
                            });
                }
                Fitness += _engine.ComputeFitness(Position, destiny);
            }
        }

        private bool Move(int index, Position destiny, Map map)
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
            if (!ValidatePosition(temp, map))
            {
                return false;
            }
            Position = temp;
            return true;
        }

        private bool ValidatePosition(Position position, Map map)
        {
            if ( position.X < 0 || position.X >= map.Width )
                return false;

            if (position.Y < 0 || position.Y >= map.Height )
                return false;

            return !map.GetNode(position).Obstacle;
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
}

