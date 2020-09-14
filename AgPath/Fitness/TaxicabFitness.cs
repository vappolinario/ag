using System;

namespace AgPath.Fitness
{
    public class TaxicabFitness : IFitnessEngine
    {
        public double ComputeFitness(Position current, Position destiny)
        {
            var xDistance = Math.Abs(destiny.X - current.X);
            var yDistance = Math.Abs(destiny.Y - current.Y);
            var distance = xDistance + yDistance;
            return 1f/distance;
        }
    }
}

