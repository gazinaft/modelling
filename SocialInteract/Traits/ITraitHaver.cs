namespace Traits;

public interface ITraitHaver
{
    public bool Busy { get; set; }
    public int Id { get; set; }
    public void Update(float delta);

    public Dictionary<string, float> GetInfo(float delta);
    public Dictionary<string, float> GetResult(float elapsed);
}