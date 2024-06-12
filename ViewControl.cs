using Chrona.Engine.Core.Interfaces;
using Chrona.Engine.Godot.Utilties;
using Godot;
using System;
using System.Collections.Generic;

namespace Chrona.Engine.Godot;

public abstract partial class ViewControl : Control
{
    public static Action<IMessage> SendCommand;
    private static List<ViewControl> list = new List<ViewControl>();

    public ISession Session => GetNode<Global>("/root/Chrona_Global").Chroncle.Session;

    public bool IsInitialized { get; private set; } = false;
    public bool IsDirty { get; private set; }

    public ViewControl()
    {
        SendCommand += (message) =>
        {
            if (message is not Message_UIRefresh)
            {
                Session.OnMessage(message);
            }

            foreach (var item in list)
            {
                item.IsDirty = true;
            }
        };

        this.Ready += () =>
        {
            LOG.INFO($"VIEW Ready {this.GetType().Name} ");

            IsDirty = true;
        };

        this.TreeEntered += () =>
        {
            list.Add(this);
        };

        this.TreeExiting += () =>
        {
            list.Remove(this);
        };
    }

    public override void _Process(double delta)
    {
        if (!IsVisibleInTree())
        {
            IsDirty = true;

            return;
        }

        if (!IsDirty)
        {
            return;
        }

        IsDirty = false;

        if (!IsInitialized)
        {
            IsInitialized = true;

            Initialize();
        }

        Update();
    }

    protected abstract void Initialize();
    protected abstract void Update();
}

public class Message_UIRefresh : IMessage
{
    public object Target { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public object Value { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
}