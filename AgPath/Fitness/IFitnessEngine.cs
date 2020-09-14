namespace AgPath.Fitness
{
    public interface IFitnessEngine
    {
        double ComputeFitness(Position current, Position destiny);
    }
}
