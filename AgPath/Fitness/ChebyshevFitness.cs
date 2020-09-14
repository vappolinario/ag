using System;

namespace AgPath.Fitness
{
    public class ChebyshevFitness : IFitnessEngine
    {
        public double ComputeFitness(Position current, Position destiny)
        {
            var xDistance = Math.Abs(destiny.X - current.X);
            var yDistance = Math.Abs(destiny.Y - current.Y);
            var distance = Math.Max(xDistance, yDistance);
            return 1f / distance;
        }
    }
}
