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

        var mockDataType = item.GetType().GetInterfaces().Single(x=>x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IMockData<>));

        var mock = mockDataType.GetProperty("Mock").GetValue(item, null);
        return (T)mock;
    }

    public static void RefreshItems<T2>(this InstancePlaceholder prototype, IEnumerable<T2> objects, Action<CanvasItem, T2> action = null)
    {
        var items = prototype.GetParent().GetChildren()
            .OfType<CanvasItem>()
            .Where(x => x.Name.ToString().Contains($"{prototype.Name}")).ToList();

        var changeCount = items.Count() - objects.Count();
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
