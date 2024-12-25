using Chrona.Engine.Core;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Chrona.Engine.Godot;

public interface IView
{
    static Dictionary<IView, uint> view2Label = new Dictionary<IView, uint>();
    static HashSet<IView> selfDirtyViews = new HashSet<IView>();

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
            view2Label.Add(this, Decorator.Label);
            var node = this as Node;

            node.Connect(Node.SignalName.TreeExiting, Callable.From(() => view2Label.Remove(this)));
            return true;
        }

        view2Label[this] = Decorator.Label;
        return value != Decorator.Label;
    }

}

public static class CanvasItemExtension
{
    static Dictionary<CanvasItem, uint> view2Label = new Dictionary<CanvasItem, uint>();

    public static bool IsDirty(this CanvasItem item)
    {
        if (!view2Label.TryGetValue(item, out uint value))
        {
            view2Label.Add(item, Decorator.Label);

            item.Connect(Node.SignalName.TreeExiting, Callable.From(() => view2Label.Remove(item)));
            return true;
        }

        view2Label[item] = Decorator.Label;
        return value != Decorator.Label;
    }

    public static T GetSession<T>(this CanvasItem item)
    {
        var chroncle = item.GetNode<Global>("/root/Chrona_Global").Chroncle;
        if(chroncle.Session != null)
        {
            return (T)chroncle.Session;
        }

        var mockDataType = item.GetType().GetInterfaces().Single(x=>x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IMockData<>));

        var mock = mockDataType.GetProperty("Mock").GetValue(item, null);
        return (T)mock;
    }

    public static void RefreshItems<T1, T2>(this T1 prototype, IEnumerable<T2> objects, Action<T1, T2> action = null)
        where T1 : CanvasItem
    {
        var items = prototype.GetParent().GetChildren()
            .OfType<T1>()
            .Where(x => x.Name.ToString().Contains($"{prototype.Name}")).ToList();

        var changeCount = items.Count() - objects.Count();
        if (changeCount > 0)
        {
            foreach(var item in items.TakeLast(changeCount))
            {
                item.Visible = false;
            }
        }
        else if(changeCount < 0)
        {
            for(int i=0; i< Math.Abs(changeCount); i++)
            {
                var newItem = prototype.Duplicate() as T1;
                prototype.AddSibling(newItem);
                items.Add(newItem);
            }
        }

        for(int i=0; i< objects.Count(); i++)
        {
            action?.Invoke(items[i], objects.ElementAt(i));
            items[i].Visible = true;
        }
    }
}

public interface IMockData<T>
    where T : class, new()
{
    T Mock => new T();
}