using System.Linq;
using System.Collections.Generic;
using System.Collections;
using AgPath.Fitness;

namespace AgPath
{
    public class Population
    {
        public List<Gene> Individuals { get; set; }
        private readonly IFitnessEngine _engine;

        public Population(int size, int maxRoute, IFitnessEngine engine)
        {
            _engine = engine;
            Individuals = Enumerable
                .Range(1, size)
                .Select(_ => new Gene(maxRoute * 2, engine)) // two bits per movement
                .ToList();
        }

        public void Check(Position currentPosition, Position destiny, Map map)
        {
            foreach (var gene in Individuals)
            {
                gene.IterateRoute(currentPosition, destiny, map);
            }
            Individuals.Sort();
        }

        public void Cross()
        {
            var result = new List<Gene>();
            var parents = Individuals.GetRange(2, (int)(Individuals.Count() * 0.2f));
            var percent = (int)(Individuals.Count() * 0.15f);
            var childrenOfFirst = Individuals.GetRange(2, percent);
            var childrenOfSecond = Individuals.GetRange(2 + percent, percent);
            var rest = Individuals.Skip(parents.Count() + 2);

            result.Add(Individuals[0]);
            result.Add(Individuals[1]);
            result.AddRange(parents);
            result.AddRange(CrossChildren(childrenOfFirst, Individuals[0]));
            result.AddRange(CrossChildren(childrenOfSecond, Individuals[1]));

            var parent = 0;
            foreach (var item in rest)
            {
                parent = parent > percent ? 0 : parent++;
                result.AddRange(CrossChildren(new List<Gene> { item }, Individuals[parent]));
            }

            Individuals = result.GetRange(0, Individuals.Count());
        }

        public void Mutate(int threshold)
        {
            Individuals.ForEach(g => g.Mutate(threshold));
        }

        private List<Gene> CrossChildren(List<Gene> children, Gene parent)
        {
            var sliceAt = parent.Chromosomes.Length / 2;
            var newGeneration = new List<Gene>();
            foreach (var gene in children)
            {
                var child = new BitArray(parent.Chromosomes.Length);
                for (int index = 0; index < child.Length; index++)
                {
                    child.Set(index,
                            index <= sliceAt
                            ? gene.Chromosomes.Get(index)
                            : parent.Chromosomes.Get(index));
                }
                var newGene = new Gene(child.Length, _engine);
                gene.Chromosomes = child;
                newGeneration.Add(newGene);
            }
            return newGeneration;
        }

        public override string ToString()
        {
            return $"[{Individuals.Count}]";
        }
    }
}
