using SocialInteract.Rng;
using SocialInteract.SocialPoints;
using Traits;

namespace SocialInteract;

static class Program
{
    static void Main(string[] args)
    {
        IRng rand = new RngNormal();
        
        var park = new PublicPlace(10, 10, 40, 10) { Name = "Cinema" };
        var office = new Job(40, 10, 10, 10) { Name = "Office" };
        var library = new ExplorePoint(10, 10, 10, 40) { Name = "Library" };
        var dayoff = new RestPlace(10, 10, 10, 40) { Name = "Day Off Dinner And Sleep" };

        var gen = new PopulationGenerator(library, office, park, dayoff);
        gen.SetRandom(rand, rand, rand, rand);

        var traitPoints = new List<ITraitPoint<SocialTraitHaver>> { park, office, library, dayoff };
        var traitHavers = gen.GeneratePopulation(100);
        
        ISimModel<SocialTraitHaver> sim = new SocialModel
        {
            TraitPoints = traitPoints,
            TraitHavers = traitHavers
        };
        
        sim.Simulate(10000, 10);
    }
}