//using Chrona.Engine.Core;
//using Godot;
//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace Chrona.Engine.Godot;

//public interface IContainer
//{
//    void Refresh<T>(Node node, IEnumerable<object> objects, Action)
//    {
//        var items = node.GetParent().GetChildren().Where(x=> x.Name)

//        foreach (var item in items.Where(x => !objects.Contains(x.Id)).ToArray())
//        {
//            item.QueueFree();
//        }

//        foreach (var obj in objects.Except(items.Select(x => x.Id)).ToArray())
//        {
//            var newItem = placeholder.CreateInstance() as T;
//            newItem.Id = obj;
//        }
//    }
//}
