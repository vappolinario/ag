using System;

namespace Tank
{
    class Program
    {
        static void Main(string[] args)
        {
            var map = new Map(10, 10);
            var population = new Population(50);
            var start = new Position { X = 0, Y = 0 };
            var destiny = new Position { X = 9, Y = 9 };

            System.Console.WriteLine(map);
            population.Check(start, destiny, map);

            for (int i = 0; i < 40; i++)
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
