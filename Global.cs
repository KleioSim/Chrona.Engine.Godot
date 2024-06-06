using Chrona.Engine.Core;
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

    public T GetSession<T>()
        where T : ISession
    {
        return (T)session;
    }

    public void SetSession(ISession session)
    {
        this.session = session;
        this.session.Modder = Modder;

        Event.ProcessMessage = session.OnMessage;
    }

    public IModder Modder { get; set; } = new Modder(ModPath);

    private ISession session;
}
