using Chrona.Engine.Core;
using Godot;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Chrona.Engine.Godot;

public interface IBinding<T> : IView
{

}

public interface IBinding<T1, T2> : IView
{

}

public static class IBindingExtension
{
    static Dictionary<IView, object> bindingDict = new Dictionary<IView, object>();
    public static void SetBinding<T>(this IBinding<T> target, T data)
    {
        var item = target as CanvasItem;

        if (!bindingDict.ContainsKey(target))
        {
            item.Connect(Node.SignalName.TreeExiting, Callable.From(() => bindingDict.Remove(target)));
        }

        var viewManager = item.GetNode<ViewManager>("/root/Chrona_ViewMgr");
        viewManager.SetDirty(target);

        bindingDict[target] = data;
    }

    public static T GetBinding<T>(this IBinding<T> target)
    {
        var item = target as CanvasItem;
        if (bindingDict.TryGetValue(target, out var value))
        {
            return (T)value;
        }

        var mockDataType = item.GetType().GetInterfaces().Single(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IMockBinding<>));
        var mock = mockDataType.GetProperty("Mock").GetValue(item, null);
        return (T)mock;
    }
}