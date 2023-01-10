namespace Traits;

public abstract class ITraitPoint<T> where T : ITraitHaver
{
    public string Name { get; set; }
    public int ProcessedQuantity { get; set; }
    public float MeanBusiness { get; set; }
    public List<T> TraitHavers { get; set; }

    public float CountInfluence(List<ITrait> traits)
    {
        return traits.Sum(trait => trait.Degree) / traits.Count;
    }
    public abstract void Interaction(List<T> traitHavers);

    public void Update(float delta)
    {
        ProcessedQuantity += TraitHavers.Count;
        MeanBusiness += TraitHavers.Count * delta;
        Interaction(TraitHavers);
        TraitHavers.Clear();
    }

    public float ZeroOrMore(float value)
    {
        return float.Max(value, 0f);
    }
    
    public Dictionary<string, float> GetInfo()
    {
        var res = new Dictionary<string, float>
        {
            { "Processed", ProcessedQuantity }
        };
        return res;
    }

    public Dictionary<string, float> GetStatistics(float elapsed)
    {
        var res = new Dictionary<string, float>
        {
            { "TotalPeople", ProcessedQuantity },
            { "MeanBusiness", MeanBusiness / elapsed }
        };
        return res;
    }
}