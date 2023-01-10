using Traits;

namespace SocialInteract.SocialTraits;

public class Aggressive : ITrait
{
    public Aggressive(float degree)
    {
        Degree = (2 * degree) - 1;
    }

    public float Degree { get; }
}