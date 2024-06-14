namespace Chrona.Engine.Godot.UBBCodes;

public static class UBB
{
    public static UBBWapper Core(object obj)
    {
        return new UBBCore(obj);
    }

    public static UBBWapper ColorNumber(float num)
    {
        return new UBBCore(num.ToString()).Color(num < 0 ? UBBColor.RED : UBBColor.GREEN);
    }
}
