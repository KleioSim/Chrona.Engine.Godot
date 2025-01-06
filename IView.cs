using Chrona.Engine.Core;
using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Chrona.Engine.Godot;

public interface IView
{
    void Refresh();
}

public static class CanvasItemExtension
{
    public static T GetSession<T>(this CanvasItem item)
    {
        var chroncle = item.GetNode<Global>("/root/Chrona_Global").Chroncle;
        if(chroncle.Session != null)
        {
            return (T)chroncle.Session;
        }

        var mockDataType = item.GetType().GetInterfaces().Single(x=>x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IMockSession<>));

        var mock = mockDataType.GetProperty("Mock").GetValue(item, null);
        return (T)mock;
    }

    public static void SetSession<T>(this CanvasItem item, T session)
    {
        var chroncle = item.GetNode<Global>("/root/Chrona_Global").Chroncle;
        chroncle.Session = session;
    }

    public static void RefreshItems<T2>(this InstancePlaceholder prototype, IEnumerable<T2> objects, Action<CanvasItem, T2> action = null, int minCount = 0)
    {
        var items = prototype.GetParent().GetChildren()
            .OfType<CanvasItem>()
            .Where(x => x.Name.ToString().Contains($"{prototype.Name}")).ToList();

        var needCount = Math.Max(objects.Count(), minCount);
        var changeCount = items.Count() - needCount;
        if (changeCount > 0)
        {
            foreach(var item in items.TakeLast(changeCount))
            {
                item.QueueFree();
            }
        }
        else if(changeCount < 0)
        {
            for(int i=0; i< Math.Abs(changeCount); i++)
            {
                var newItem = prototype.CreateInstance() as CanvasItem;
                newItem.Name = prototype.Name;
                items.Add(newItem);

                newItem.SetMeta("NewFlag", true);
            }
        }

        for(int i=0; i< needCount; i++)
        {
            action?.Invoke(items[i], objects.Count() > i ? objects.ElementAt(i) : default(T2));
            items[i].Visible = true;
            items[i].SetMeta("NewFlag", false);
        }
    }
}

public interface IMockBinding<T>
    where T : class, new()
{
    T Mock => new T();
}

public interface IMockSession<T>
        where T : class, new()
{
    T Mock => new T();
}