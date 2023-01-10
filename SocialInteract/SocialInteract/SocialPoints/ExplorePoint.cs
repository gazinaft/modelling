using Traits;

namespace SocialInteract.SocialPoints;

public class ExplorePoint : ITraitPoint<SocialTraitHaver>
{
    private float _moneySpend;
    private float _restSpend;
    private float _happySpend;
    private float _exploreGained;
    
    public ExplorePoint(float moneySpend, float restSpend, float happySpend, float exploreGained)
    {
        _moneySpend = moneySpend;
        _restSpend = restSpend;
        _happySpend = happySpend;
        _exploreGained = exploreGained;
        TraitHavers = new List<SocialTraitHaver>();
    }

    public override void Interaction(List<SocialTraitHaver> traitHavers)
    {
        foreach (var traitHaver in traitHavers)
        {
            traitHaver.Money -= _moneySpend;
            traitHaver.Rest -= _restSpend + 0.5f * _restSpend * traitHaver.Hardworking.Degree;
            traitHaver.Happiness -= _happySpend;
            traitHaver.Exploring += _exploreGained + 0.5f * _exploreGained * traitHaver.Hardworking.Degree;
            traitHaver.Busy = false;

            traitHaver.Money = ZeroOrMore(traitHaver.Money);
            traitHaver.Rest = ZeroOrMore(traitHaver.Rest);
            traitHaver.Happiness = ZeroOrMore(traitHaver.Happiness);
            traitHaver.Exploring = ZeroOrMore(traitHaver.Exploring);
        }
    }
    
}