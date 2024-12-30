using Chrona.Engine.Core;
using Chrona.Engine.Core.Events;
using Chrona.Engine.Core.Interfaces;
using Chrona.Engine.Core.Modders;
using Chrona.Engine.Godot.Utilties;
using Godot;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using GodotEngine = Godot.Engine;

namespace Chrona.Engine.Godot;

public partial class Global : Node
{
    public Chroncle Chroncle { get; private set; }

    public override void _Ready()
    {
        var modPath = Path.Combine(ProjectSettings.GlobalizePath("user://"), "mods");

        //CreateNativeMode(modPath);
        //Chroncle = new Chroncle(new Modder(modPath));
        Chroncle = new Chroncle(null);
    }

    private void CreateNativeMode(string modPath)
    {
        var targetPath = Path.Combine(modPath, "native/dll");
        Directory.Delete(targetPath, true);
        Directory.CreateDirectory(targetPath);

        var sourcePath = OS.HasFeature("editor") ?
            Path.Combine(ProjectSettings.GlobalizePath("res://"), ".godot/mono/temp/bin/Debug")
          : Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        var files = Directory.EnumerateFiles(sourcePath, "*Native*");
        if (files.Any())
        {
            foreach (var file in files)
            {
                File.Copy(file, Path.Combine(targetPath, Path.GetFileName(file)), true);
            }
        }
    }
}