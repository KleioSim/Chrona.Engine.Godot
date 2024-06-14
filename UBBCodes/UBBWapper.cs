using System;

namespace Chrona.Engine.Godot.UBBCodes;

public abstract class UBBWapper
{
    protected object content;

    protected object wapperValue;

    public UBBWapper(object content)
    {
        this.content = content;
    }

    public UBBWapper Color(UBBColor color)
    {
        return new UBBWapperColor(this) { wapperValue = color };
    }
}

public class UBBWapperColor : UBBWapper
{
    public UBBWapperColor(object content) : base(content)
    {
    }

    public override string ToString()
    {
        var color = (UBBColor)wapperValue;

        return $"[color=#{color.GetAttributeOfType<ColorCodeAttribute>().value}]{content}[/color]";
    }
}


public static class EnumHelper
{
    /// <summary>
    /// Gets an attribute on an enum field value
    /// </summary>
    /// <typeparam name="T">The type of the attribute you want to retrieve</typeparam>
    /// <param name="enumVal">The enum value</param>
    /// <returns>The attribute of type T that exists on the enum value</returns>
    /// <example><![CDATA[string desc = myEnumVariable.GetAttributeOfType<DescriptionAttribute>().Description;]]></example>
    public static T GetAttributeOfType<T>(this Enum enumVal) where T : System.Attribute
    {
        var type = enumVal.GetType();
        var memInfo = type.GetMember(enumVal.ToString());
        var attributes = memInfo[0].GetCustomAttributes(typeof(T), false);
        return (attributes.Length > 0) ? (T)attributes[0] : null;
    }
}