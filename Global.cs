using Chrona.Engine.Core;
using Chrona.Engine.Core.Events;
using Chrona.Engine.Core.Interfaces;
using Chrona.Engine.Core.Modders;
using Godot;
using System.IO;

namespace Chrona.Engine.Godot;

public partial class Global : Node
{
    public static string ModPath
    {
        get
        {
            var path = Path.Combine(Path.GetDirectoryName(OS.GetExecutablePath()), "mods");
            if (OS.HasFeature("editor"))
            {
                path = Path.Combine(ProjectSettings.GlobalizePath("res://"), ".mods");
            }

            return path;
        }
    }

    public Chroncle Chroncle { get; } = new Chroncle(new Modder(ModPath));
}
