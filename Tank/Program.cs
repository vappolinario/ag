using System;
using System.CommandLine.DragonFruit;

namespace Tank
{
    class Program
    {
        /// <param name="width">Map width</param>
        /// <param name="height">Map height</param>
        /// <param name="populationSize">Size of each generation</param>
        static void Main(int width = 10, int height = 10, int populationSize = 50)
        {
            var map = new Map(width, height, 0);
            var mapTiles = width + height;
            var population = new Population(populationSize, (int)(mapTiles*1.5));
            var start = new Position { X = 0, Y = 0 };
            var destiny = new Position { X = width-1, Y = height-1 };

            //System.Console.WriteLine(map);
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
