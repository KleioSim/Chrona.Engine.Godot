using Chrona.Engine.Core.Events;
using Godot;

namespace Chrona.Engine.Godot.EventDialog;

[Tool]
public partial class DialogFacade : ViewControl
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
            var dialog = Dialog.CreateInstance() as Dialog;
            dialog.eventObj = @event;

            GD.Print("Visible");

            await ToSignal(dialog, Control.SignalName.TreeExited);

            GD.Print("await");

        }

        EmitSignal(SignalName.TurnEnd);

        SendCommand(new Message_UIRefresh());
    }

    protected override void Initialize()
    {

    }

    protected override void Update()
    {

    }
}