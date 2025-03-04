public class SinFunctionDivided : SinFunction
{
    private readonly int _value = 0;

    public SinFunctionDivided(int value)
    {
        _value = value;
    }

    public override float Math(float x)
    {
        return base.Math(_value * x) / _value;
    }
}