using Traits;

namespace SocialInteract.SocialTraits;

public class Hardworking : ITrait
{
    public Hardworking(float degree)
    {
        Degree = (2 * degree) - 1;
    }

    public float Degree { get; }
}