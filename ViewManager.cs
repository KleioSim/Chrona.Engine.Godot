using Chrona.Engine.Core;
using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Chrona.Engine.Godot;

internal partial class ViewManager : Node
{
    private Dictionary<IView, uint> view2Label = new Dictionary<IView, uint>();

    public override void _EnterTree()
    {
        this.GetTree().Connect(SceneTree.SignalName.ProcessFrame, Callable.From(() =>
        {
            foreach(var pair in view2Label.ToArray())
            {
                var canvaItem = pair.Key as CanvasItem;
                if (!canvaItem.Visible)
                {
                    continue;
                }

                if(pair.Value != Decorator.Label)
                {
                    pair.Key.Refresh();
                }

                view2Label[pair.Key] = Decorator.Label;
            }
        }));

        this.GetTree().Connect(SceneTree.SignalName.NodeAdded, Callable.From((Node node) =>
        {
            if (node is IView view)
            {
                view2Label.Add(view, Decorator.Label-1);
            }
        }));

        this.GetTree().Connect(SceneTree.SignalName.NodeRemoved, Callable.From((Node node) =>
        {
            if (node is IView view)
            {
                view2Label.Remove(view);
            }
        }));
    }

    internal void SetDirty(IView target)
    {
        view2Label[target] = Decorator.Label-1;
    }
}
