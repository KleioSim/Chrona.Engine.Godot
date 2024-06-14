using System;

namespace Chrona.Engine.Godot.UBBCodes;

public enum UBBColor
{
    [ColorCode("FF0000")]
    RED,

    [ColorCode("7FFF00")]
    GREEN
}

public class ColorCodeAttribute : Attribute
{
    public readonly string value;
    public ColorCodeAttribute(string value)
    {
        this.value = value;
    }
}