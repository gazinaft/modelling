namespace Traits;

public interface ISimModel<T> where T : ITraitHaver
{
    public float Elapsed { get; set; }
    public List<ITraitPoint<T>> TraitPoints { get; set; }
    public List<T> TraitHavers { get; set; }
    public int PopulationSize { get; set; }

    public void Simulate(float time, float delta)
    {
        Elapsed = 0;
        PopulationSize = TraitHavers.Count;
        while (Elapsed < time)
        {
            Update(delta);
            PrintInfo(delta);
            Elapsed += delta;
        }
        PrintResult();
    }
    
    public void Update(float delta);

    public void PrintInfo(float delta);
    public void PrintResult();
}