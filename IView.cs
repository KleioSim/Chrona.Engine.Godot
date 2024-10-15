using Chrona.Engine.Core;
using Godot;
using System.Collections.Generic;

namespace Chrona.Engine.Godot;

public interface IView<T> : IView
{
    bool IsSelfDirty
    {
        get
        {
            return selfDirtyViews.Contains(this);
        }
        set
        {
            if (value)
            {
                selfDirtyViews.Add(this);
            }
            else
            {
                selfDirtyViews.Remove(this);
            }
        }
    }

    bool IsDirty()
    {
        if (IsSelfDirty)
        {
            selfDirtyViews.Remove(this);
            return true;
        }

        if (!view2Label.TryGetValue(this, out uint value))
        {
            view2Label.Add(this, Decorator<T>.Label);
            var node = this as Node;
            node.Connect(Node.SignalName.TreeExiting, Callable.From(() => view2Label.Remove(this)));
            return true;
        }

        view2Label[this] = Decorator<T>.Label;
        return value != Decorator<T>.Label;
    }

}

public interface IView
{
    static Dictionary<IView, uint> view2Label = new Dictionary<IView, uint>();
    static HashSet<IView> selfDirtyViews = new HashSet<IView>();
}