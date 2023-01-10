namespace SocialInteract.Rng;

public class RngConst: IRng
{
    private float _value;
    
    public RngConst(float value)
    {
        this._value = value;
    }


    public float Next()
    {
        return _value;
    }

    public float Next(float max)
    {
        return _value;
    }
}