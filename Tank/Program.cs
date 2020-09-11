using System;

namespace Tank
{
    class Program
    {
        static void Main(string[] args)
        {
            var population = new Population(50);
            var start = new Position { X = 0, Y = 0 };
            var destiny = new Position { X = 9, Y = 9 };

            population.Check(start, destiny);

            for (int i = 0; i < 40; i++)
            {
                population.Cross();
                population.Mutate(12);
                population.Check(start, destiny);

            }
            Print(population);
        }

        private static void Print(Population population)
        {
            Console.WriteLine(population);
            foreach (var individual in population.Individuals)
            {
                Console.WriteLine(individual);
            }
        }
    }
}
