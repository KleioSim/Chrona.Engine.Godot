namespace Chrona.Engine.Godot.UBBCodes;

public class UBBCore : UBBWapper
{
    public UBBCore(object content) : base(content)
    {

    }

    public override string ToString()
    {
        return content.ToString();
    }
}
