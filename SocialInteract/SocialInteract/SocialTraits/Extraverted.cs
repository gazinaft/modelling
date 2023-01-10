using Traits;

namespace SocialInteract.SocialTraits;

public class Extraverted : ITrait
{
    public Extraverted(float degree)
    {
        Degree = (2 * degree) - 1;
    }

    public float Degree { get; }
}