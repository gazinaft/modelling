using MathNet.Numerics.Distributions;

namespace SocialInteract.Rng;

public class RngNormal : IRng
{

    private Random _random;
    private Normal _norm;
    
    public RngNormal(float mean = 0.5f, float stddev = 0.25f)
    {
        _random = new Random();
        _norm = new Normal(mean, stddev, _random);
    }


    public float Next()
    {
        return (float)Math.Clamp(_norm.Sample(), 0, 1);
    }

    public float Next(float max)
    {
        return max * (float)Math.Clamp(_norm.Sample(), 0, 1);
    }
}