using Traits;

namespace SocialInteract.SocialPoints;

public class RestPlace : ITraitPoint<SocialTraitHaver>
{

    private float _moneySpend;
    private float _exploringSpend;
    private float _happySpend;
    private float _restGained;
    
    public RestPlace(float moneySpend, float exploringSpend, float happySpend, float restGained)
    {
        _moneySpend = moneySpend;
        _exploringSpend = exploringSpend;
        _happySpend = happySpend;
        _restGained = restGained;
        
        TraitHavers = new List<SocialTraitHaver>();

    }
    
    public override void Interaction(List<SocialTraitHaver> traitHavers)
    {
        foreach (var traitHaver in traitHavers)
        {
            traitHaver.Money -= _moneySpend;
            traitHaver.Exploring -= _exploringSpend;
            traitHaver.Happiness -= _happySpend;
            traitHaver.Rest += _restGained + 0.5f * _restGained * traitHaver.Hardworking.Degree;
            traitHaver.Busy = false;
            
            traitHaver.Money = ZeroOrMore(traitHaver.Money);
            traitHaver.Rest = ZeroOrMore(traitHaver.Rest);
            traitHaver.Happiness = ZeroOrMore(traitHaver.Happiness);
            traitHaver.Exploring = ZeroOrMore(traitHaver.Exploring);
        }
        
    }
}