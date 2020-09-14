namespace AgPath.Fitness
{
    public static class FitnessFactory
    {
        public static IFitnessEngine GetEngine(Formula formula)
        {
            switch (formula)
            {
                case Formula.Chebyshev:
                    return new ChebyshevFitness();
                case Formula.Taxicab:
                    return new TaxicabFitness();
                case Formula.Euclidean:
                default:
                    return new EuclideanFitness();
            }
        }
    }
}
