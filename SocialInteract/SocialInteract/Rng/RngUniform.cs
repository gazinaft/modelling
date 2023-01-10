namespace SocialInteract.Rng;

public class RngUniform: IRng
{
    private Random _random;
    public RngUniform()
    {
        _random = new Random();
    }
    
    public float Next()
    {
        return (float)_random.NextDouble();
    }

    public float Next(float max)
    {
        return (float)_random.NextDouble() * max;
    }
}