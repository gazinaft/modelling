using SocialInteract.SocialPoints;
using SocialInteract.SocialTraits;
using Traits;

namespace SocialInteract;

public class SocialTraitHaver : ITraitHaver
{
    public float Happiness { get; set; }
    public float Money { get; set; }
    public float Rest { get; set; }
    public float Exploring { get; set; }
    
    public float CriticalNeed = 40;
    public float AbilityLevel = 10;

    private float sumHappy = 0;
    private float sumMoney = 0;
    private float sumRest = 0;
    private float sumExploring = 0;
    
    
    
    public Aggressive Aggressive { get; set; }
    public Extraverted Extraverted { get; set; }
    public Hardworking Hardworking { get; set; }
    public Mental Mental { get; set; }
    
    public bool Dead { get; set; }

    public bool Busy { get; set; }
    public int Id { get; set; }
    private int _day = 0;
    
    private ExplorePoint _explorePoint;
    private Job _job;
    private PublicPlace _publicPlace;
    private RestPlace _restPlace;
    
    public SocialTraitHaver(ExplorePoint explorePoint, Job job, PublicPlace publicPlace, RestPlace restPlace)
    {
        _explorePoint = explorePoint;
        _job = job;
        _publicPlace = publicPlace;
        _restPlace = restPlace;

        Dead = false;
        
    }

    float NeedCoef(float need)
    {
        if (need >= CriticalNeed) return 1f;
        return Single.Sqrt(CriticalNeed - need);
    }

    ITraitPoint<SocialTraitHaver> NumToPoint(int number)
    {
        return number switch
        {
            0 => _explorePoint,
            1 => _job,
            2 => _publicPlace,
            3 => _restPlace,
            _ => _job
        };
    }

    bool CanLive()
    {
        List<float> needs = new List<float>
        {
          Happiness,
          Exploring,
          Rest,
          Money
        };

        return needs.Count(x => x < AbilityLevel) < 2;
    }

    public void Update(float delta)
    {
        if (Busy) return;
        _day += 1;
        List<float> priorities = new List<float>
        {
            NeedCoef(Exploring) + 0.5f * Hardworking.Degree,
            NeedCoef(Money) + Hardworking.Degree,
            NeedCoef(Happiness) + Extraverted.Degree,
            NeedCoef(Rest) - Mental.Degree
        };
        var max = priorities.Max();
        
        if (!CanLive())
        {
            Console.WriteLine("Citizen" + Id + " has died on day " + _day);
            Console.WriteLine(" money|" + Money + "|happiness|" + Happiness + "|rest|" + Rest + "|exploring|" + Exploring);
            this.Dead = true;
            // Model?.TraitHavers.Remove(this);
            return;
        }
        for (int i = 0; i < priorities.Count; i++)
        {
            if (max == priorities[i])
            {
                NumToPoint(i).TraitHavers.Add(this);
                Busy = true;
                return;
            }
        }
    }

    public Dictionary<string, float> GetInfo(float delta)
    {
        sumExploring += Exploring * delta;
        sumHappy += Happiness * delta;
        sumMoney += Money * delta;
        sumRest += Rest * delta;
        return new Dictionary<string, float>();
    }

    public Dictionary<string, float> GetResult(float elapsed)
    {
        var res = new Dictionary<string, float>
        {
            { "Money", Money },
            { "MeanMoney", sumMoney / elapsed },
            { "MeanHappy", sumHappy/elapsed },
            { "MeanExploring", sumExploring/elapsed },
            { "MeanRest", sumRest/elapsed }
        };

        return res;
    }

}
