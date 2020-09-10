using System.Linq;
using System.Collections.Generic;

public class Population
{
    public List<Gene> Individuals { get; set; }

    public Population(int size)
    {
        Individuals = Enumerable
            .Range(1, size)
            .Select(_ => new Gene())
            .ToList();
    }

    public void Check(Position currentPosition, Position destiny)
    {
        foreach (var gene in Individuals)
        {
            gene.IterateRoute(currentPosition, destiny);
        }
        Individuals.Sort();
    }

    public void Cross()
    {
        var result = new List<Gene>();
        var parents = Individuals.GetRange(2, Individuals.Count()*20/100);
        var percent = Individuals.Count()*15/100;
        var childrenOfFirst = Individuals.GetRange(2, percent);
        var childrenOfSecond = Individuals.GetRange(2+percent, percent);
        var rest = Individuals.Skip(parents.Count()+2);

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
        var sliceAt = parent.Chromosomes.Count()/2;
        var newGeneration = new List<Gene>();
        foreach (var gene in children)
        {
            var child = gene
                .Chromosomes
                .Select((chromosome, index) =>
                        {
                        return index <= sliceAt ? chromosome : parent.Chromosomes[index];
                        })
                .ToList();
            newGeneration.Add(new Gene { Chromosomes = child });
        }
        return newGeneration;
    }

    public override string ToString()
    {
        return $"[{Individuals.Count}]";
    }
}
