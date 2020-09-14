using System;

namespace AgPath.Fitness
{
    public class EuclideanFitness : IFitnessEngine
    {
        public double ComputeFitness(Position current, Position destiny)
        {
            var distance = Math.Pow(destiny.X - current.X, 2);
            distance += Math.Pow(destiny.Y - current.Y, 2);
            distance = Math.Sqrt(distance);
            return 1f/distance;
        }
    }
}

