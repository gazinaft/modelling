using MathNet.Numerics.Statistics;
using Traits;

namespace SocialInteract;

public class SocialModel : ISimModel<SocialTraitHaver>
{
    public float Elapsed { get; set; }
    public List<ITraitPoint<SocialTraitHaver>> TraitPoints { get; set; }
    public List<SocialTraitHaver> TraitHavers { get; set; }
    public int PopulationSize { get; set; }

    public void Update(float delta)
    {
        foreach (var traitHaver in TraitHavers)
        {
            traitHaver.Update(delta);
        }
        TraitHavers = TraitHavers.Where(x => !x.Dead).ToList();
        foreach (var traitPoint in TraitPoints)
        {
            traitPoint.Update(delta);
        }
    }

    public void PrintInfo(float delta)
    {
        foreach (var traitHaver in TraitHavers)
        {
            traitHaver.GetInfo(delta);
        }
        foreach (var traitPoint in TraitPoints)
        {
            traitPoint.GetInfo();
        }
    }

    public void PrintResult()
    {
        List<Dictionary<string, float>> aggregatedHavers = new List<Dictionary<string, float>>();
        
        foreach (var traitHaver in TraitHavers)
        {
            aggregatedHavers.Add(traitHaver.GetResult(Elapsed));
        }

        var died = PopulationSize - TraitHavers.Count;
        Console.WriteLine("Died in process of simulation: " + died);

        var finalMoney = aggregatedHavers.Select(x => x["Money"]).Mean();
        var meanMoney = aggregatedHavers.Select(x => x["MeanMoney"]).Mean();
        var meanHappy = aggregatedHavers.Select(x => x["MeanHappy"]).Mean();
        var meanExploring = aggregatedHavers.Select(x => x["MeanExploring"]).Mean();
        var meanRest = aggregatedHavers.Select(x => x["MeanRest"]).Mean();

        Console.WriteLine("Mean amount of money at the end of simulation: " + finalMoney);
        Console.WriteLine("Mean amount of money throughout of simulation: " + meanMoney);
        Console.WriteLine("Mean amount of happiness throughout of simulation: " + meanHappy);
        Console.WriteLine("Mean amount of exploring throughout of simulation: " + meanExploring);
        Console.WriteLine("Mean amount of rest throughout of simulation: " + meanRest);

        Console.WriteLine("==============================================");
        foreach (var traitPoint in TraitPoints)
        {
            Console.WriteLine();
            Console.WriteLine("Point " + traitPoint.Name);
            var textResults = traitPoint.GetStatistics(Elapsed);
            Console.WriteLine("Total people at this point: " + textResults["TotalPeople"]);
            Console.WriteLine("Mean business of this point: " + textResults["MeanBusiness"]);
            
        }
        
    }

}