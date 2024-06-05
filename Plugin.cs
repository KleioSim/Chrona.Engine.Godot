#if TOOLS
using Godot;

namespace Chrona.Engine.Godot;

[Tool]
public partial class Plugin : EditorPlugin
{
    public override void _EnterTree()
    {
        AddAutoloadSingleton("Chrona_Global", "res://addons/Chrona.Engine.Godot/Global.cs");

        var script = GD.Load<Script>("res://addons/Chrona.Engine.Godot/EventDialog/DialogFacade.cs");
        var texture = GD.Load<Texture2D>("res://addons/Chrona.Engine.Godot/Icon.png");
        AddCustomType("Chrona.EventDialog", "Control", script, texture);
    }

    public override void _ExitTree()
    {
        RemoveCustomType("Chrona.EventDialog");
    }
}
#endif