using Traits;

namespace SocialInteract.SocialPoints;

public class PublicPlace : ITraitPoint<SocialTraitHaver>
{
    private float _moneySpend;
    private float _exploringSpend;
    private float _happyGained;
    private float _restSpend;
    
    public PublicPlace(float moneySpend, float exploringSpend, float happyGained, float restSpend)
    {
        _moneySpend = moneySpend;
        _exploringSpend = exploringSpend;
        _happyGained = happyGained;
        _restSpend = restSpend;
        
        TraitHavers = new List<SocialTraitHaver>();

    }
    
    public override void Interaction(List<SocialTraitHaver> traitHavers)
    {
        foreach (var traitHaver in traitHavers)
        {
            traitHaver.Money -= _moneySpend;
            traitHaver.Exploring -= _exploringSpend;
            traitHaver.Happiness += _happyGained + traitHaver.Extraverted.Degree * 0.5f * _happyGained;
            traitHaver.Rest -= _restSpend + traitHaver.Aggressive.Degree * 0.5f * _restSpend;
            traitHaver.Busy = false;
            
            traitHaver.Money = ZeroOrMore(traitHaver.Money);
            traitHaver.Rest = ZeroOrMore(traitHaver.Rest);
            traitHaver.Happiness = ZeroOrMore(traitHaver.Happiness);
            traitHaver.Exploring = ZeroOrMore(traitHaver.Exploring);
        }
        
    }

}