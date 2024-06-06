using Chrona.Engine.Core.Events;
using Chrona.Engine.Core.Interfaces;
using Godot;
using HuangD.Sessions;
using System.Threading.Tasks;

namespace Chrona.Engine.Godot.EventDialog;

[Tool]
public partial class DialogFacade : Control
{
    public DialogFacade()
    {
        var controlPanel = GD.Load<PackedScene>("res://addons/Chrona.Engine.Godot/EventDialog/EventDialogRoot.tscn").Instantiate();
        this.AddChild(controlPanel, true);
    }

    [Signal]
    public delegate void TurnStartEventHandler();

    [Signal]
    public delegate void TurnEndEventHandler();

    public Global Global => GetNode<Global>("/root/Chrona_Global");
    public InstancePlaceholder Dialog => GetNode<InstancePlaceholder>("EventDialogRoot/Dialog");


    public async void OnNextTurn()
    {
        EmitSignal(SignalName.TurnStart);

        foreach (Event @event in Global.Chroncle.OnNextTurn())
        {
            var dialog = Dialog.CreateInstance();

            GD.Print("Visible");

            await ToSignal(dialog, Control.SignalName.TreeExited);
            @event.DoOption();

            GD.Print("await");

        }

        EmitSignal(SignalName.TurnEnd);
    }
}