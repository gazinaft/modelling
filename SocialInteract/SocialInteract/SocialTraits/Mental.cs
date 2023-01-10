using Traits;

namespace SocialInteract.SocialTraits;

public class Mental : ITrait
{
    public Mental(float degree)
    {
        Degree = (2 * degree) - 1;
    }

    public float Degree { get; }
}