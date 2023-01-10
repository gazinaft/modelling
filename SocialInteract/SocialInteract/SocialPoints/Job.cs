using Traits;

namespace SocialInteract.SocialPoints;

public class Job : ITraitPoint<SocialTraitHaver>
{

    private float _moneyGained;
    private float _exploringSpend;
    private float _happySpend;
    private float _restSpend;
    
    public Job(float moneyGained, float exploringSpend, float happySpend, float restSpend)
    {
        _moneyGained = moneyGained;
        _exploringSpend = exploringSpend;
        _happySpend = happySpend;
        _restSpend = restSpend;
        
        TraitHavers = new List<SocialTraitHaver>();

    }
    
    public override void Interaction(List<SocialTraitHaver> traitHavers)
    {
        foreach (var traitHaver in traitHavers)
        {
            traitHaver.Money += _moneyGained + 0.5f * _moneyGained * traitHaver.Hardworking.Degree;
            traitHaver.Exploring -= _exploringSpend + 0.5f * _exploringSpend * traitHaver.Hardworking.Degree;
            traitHaver.Happiness -= _happySpend + 0.5f * _happySpend *
                                    CountInfluence(traitHavers.Select(x => x.Aggressive).ToList<ITrait>());
            traitHaver.Rest -= _restSpend - 0.5f * traitHaver.Mental.Degree * _restSpend;
            traitHaver.Busy = false;
            
            traitHaver.Money = ZeroOrMore(traitHaver.Money);
            traitHaver.Rest = ZeroOrMore(traitHaver.Rest);
            traitHaver.Happiness = ZeroOrMore(traitHaver.Happiness);
            traitHaver.Exploring = ZeroOrMore(traitHaver.Exploring);
        }
        
    }

}