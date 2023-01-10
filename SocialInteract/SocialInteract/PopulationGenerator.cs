using MathNet.Numerics.Distributions;
using SocialInteract.Rng;
using SocialInteract.SocialPoints;
using SocialInteract.SocialTraits;
using Traits;

namespace SocialInteract;

public class PopulationGenerator
{
    public List<SocialTraitHaver> GeneratePopulation(int size)
    {
        return GeneratePopulationInternal(size,
            GenerateAggressive(size, 0.5, 0.25),
            GenerateExtraverted(size, 0.5, 0.25),
            GenerateHardWorking(size, 0.5, 0.25),
            GenerateMental(size, 0.5, 0.25));
    }

    private ExplorePoint _explorePoint;
    private Job _job;
    private PublicPlace _publicPlace;
    private RestPlace _restPlace;

    private IRng _rngExplore;
    private IRng _rngMoney;
    private IRng _rngHappy;
    private IRng _rngRest;

    private const int MaxNeeds = 100;

    public void SetRandom(IRng rngExplore, IRng rngMoney, IRng rngHappy, IRng rngRest)
    {
        _rngExplore = rngExplore;
        _rngHappy = rngHappy;
        _rngMoney = rngMoney;
        _rngRest = rngRest;
    }

    public PopulationGenerator(ExplorePoint explorePoint, Job job, PublicPlace publicPlace, RestPlace restPlace)
    {
        _explorePoint = explorePoint;
        _job = job;
        _publicPlace = publicPlace;
        _restPlace = restPlace;
    }
    

    List<SocialTraitHaver> GeneratePopulationInternal(int size,
        List<Aggressive> aggressives,
        List<Extraverted> extraverteds,
        List<Hardworking> hardworkings,
        List<Mental> mentals)
    {
        var res = new List<SocialTraitHaver>();
        for (int i = 0; i < size; i++)
        {
            res.Add(new SocialTraitHaver(_explorePoint, _job, _publicPlace, _restPlace)
            {
                Id = i,
                Aggressive = aggressives[i],
                Extraverted = extraverteds[i],
                Hardworking = hardworkings[i],
                Mental = mentals[i],
                
                Exploring = _rngExplore.Next(MaxNeeds) + 10f,
                Money = _rngMoney.Next(MaxNeeds) + 10f,
                Happiness = _rngHappy.Next(MaxNeeds) + 10f,
                Rest = _rngRest.Next(MaxNeeds) + 10f,

            });
        }

        return res;
    }

    public List<Aggressive> GenerateAggressive(int size, double mean, double stddev)
    {
        var rng = new Random();
        List<Aggressive> res = new();
        for (int i = 0; i < size; i++)
        {
            res.Add(new Aggressive((float)Math.Clamp
                (Normal.Sample(rng, mean, stddev),
                    0, 1)));    
        }
        return res;
    }
    
    public List<Extraverted> GenerateExtraverted(int size, double mean, double stddev)
    {
        var rng = new Random();
        List<Extraverted> res = new();
        for (int i = 0; i < size; i++)
        {
            res.Add(new Extraverted((float)Math.Clamp
                (Normal.Sample(rng, mean, stddev),
                0, 1)));    
        }
        return res;
    }
    public List<Hardworking> GenerateHardWorking(int size, double mean, double stddev)
    {
        var rng = new Random();
        List<Hardworking> res = new();
        for (int i = 0; i < size; i++)
        {
            res.Add(new Hardworking((float)Math.Clamp
                (Normal.Sample(rng, mean, stddev),
                0, 1)));    
        }
        return res;
    }
    
    public List<Mental> GenerateMental(int size, double mean, double stddev)
    {
        var rng = new Random();
        List<Mental> res = new();
        for (int i = 0; i < size; i++)
        {
            res.Add(new Mental((float)Math.Clamp
                (Normal.Sample(rng, mean, stddev),
                0, 1)));    
        }
        return res;
    }
}
