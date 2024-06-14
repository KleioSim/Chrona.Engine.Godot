using Godot;
using System;

namespace Chrona.Engine.Godot.TooltipTrigger;

[Tool]
public partial class TooltipTrigger : Control
{
    public Func<string> funcGetToolTipString;

    public override string _GetTooltip(Vector2 atPosition)
    {
        return funcGetToolTipString != null ? funcGetToolTipString() : this.TooltipText;
    }

    public override Control _MakeCustomTooltip(string forText)
    {
        Control tooltip = ResourceLoader.Load<PackedScene>("res://addons/Chrona.Engine.Godot/TooltipTriggers/TooltipPanel.tscn").Instantiate() as Control;
        tooltip.GetNode<RichTextLabel>("RichTextLabel").Text = forText;
        return tooltip;
    }
}
