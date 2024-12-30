#if TOOLS
using Godot;

namespace Chrona.Engine.Godot;

[Tool]
public partial class Plugin : EditorPlugin
{
    public override void _EnterTree()
    {
        AddAutoloadSingleton("Chrona_Global", "res://addons/Chrona.Engine.Godot/Global.cs");
        AddAutoloadSingleton("Chrona_ViewMgr", "res://addons/Chrona.Engine.Godot/ViewManager.cs");

        var script = GD.Load<Script>("res://addons/Chrona.Engine.Godot/EventDialog/DialogFacade.cs");
        var texture = GD.Load<Texture2D>("res://addons/Chrona.Engine.Godot/Icon.png");
        AddCustomType("Chrona.EventDialog", "Control", script, texture);

        var script2 = GD.Load<Script>("res://addons/Chrona.Engine.Godot/TooltipTriggers/TooltipTrigger.cs");
        var texture2 = GD.Load<Texture2D>("res://addons/Chrona.Engine.Godot/Icon.png");
        AddCustomType("Chrona.ToolTipTrigger", "Control", script2, texture2);
    }

    public override void _ExitTree()
    {
        RemoveAutoloadSingleton("Chrona_Global");

        RemoveCustomType("Chrona.EventDialog");
        RemoveCustomType("Chrona.ToolTipTrigger");
    }
}
#endif