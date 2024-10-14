using Chrona.Engine.Core;
using Godot;
using System.Collections.Generic;

namespace Chrona.Engine.Godot;

public interface IView
{
    static Dictionary<IView, uint> view2Label = new Dictionary<IView, uint>();

    bool IsDirty()
    {
        if (!view2Label.TryGetValue(this, out uint value))
        {
            view2Label.Add(this, Decorator<IData>.Label);
            var node = this as Node;
            node.Connect(Node.SignalName.TreeExiting, Callable.From(() => view2Label.Remove(this)));
            return true;
        }

        view2Label[this] = Decorator<IData>.Label;
        return value != Decorator<IData>.Label;
    }
}