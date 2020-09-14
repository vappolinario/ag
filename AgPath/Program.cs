using AgPath.Fitness;
namespace AgPath
{
    public enum Formula
    {
        Euclidean = 0,
        Taxicab,
        Chebyshev
    }

    class Program
    {
        /// <param name="fitnessFormula">Fitness formula to be used</param>
        /// <param name="width">Map width</param>
        /// <param name="height">Map height</param>
        /// <param name="populationSize">Size of each generation</param>
        /// <param name="obstacleRate">How much obstacles on map (percent)</param>
        static void Main(Formula fitnessFormula
                        , int width = 10
                        , int height = 10
                        , int populationSize = 50
                        , double obstacleRate = 0.2f)
        {
            var map = new Map(width, height, obstacleRate);
            var mapTiles = width + height;
            var maxRoute = (int)(mapTiles * 1.5);
            var engine = FitnessFactory.GetEngine(fitnessFormula);
            var population = new Population(populationSize, maxRoute, engine);
            var start = new Position { X = 0, Y = 0 };
            var destiny = new Position { X = width - 1, Y = height - 1 };

            System.Console.WriteLine($"using fitness: {engine.GetType().FullName}");

            population.Check(start, destiny, map);

            for (int i = 0; i < mapTiles; i++)
            {
                population.Cross();
                population.Mutate(12);
                population.Check(start, destiny, map);

            }
            System.Console.WriteLine(population.Individuals[0]);
            System.Console.WriteLine(map.DrawRoute(population.Individuals[0]));
        }
    }
}
