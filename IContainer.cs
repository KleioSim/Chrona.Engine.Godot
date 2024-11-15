using Chrona.Engine.Core;
using Godot;
using System.Collections.Generic;
using System.Linq;

namespace Chrona.Engine.Godot;

public interface IContainer
{
    void Refresh<T>(InstancePlaceholder placeholder, IEnumerable<object> objects)
        where T : Node, IItem
    {
        var items = placeholder.GetParent().GetChildren().OfType<T>().ToList();

        foreach (var item in items.Where(x => !objects.Contains(x.Id)).ToArray())
        {
            item.QueueFree();
        }

        foreach (var obj in objects.Except(items.Select(x => x.Id)).ToArray())
        {
            var newItem = placeholder.CreateInstance() as T;
            newItem.Id = obj;
        }
    }
}
