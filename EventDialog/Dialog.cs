using Chrona.Engine.Core.Events;
using Godot;

namespace Chrona.Engine.Godot.EventDialog;

public partial class Dialog : Panel
{
    public Event eventObj;

    public Button Confirm => GetNode<Button>("Button");


    public override void _Ready()
    {
        Confirm.Connect(Button.SignalName.ButtonDown, new Callable(this, MethodName.OnConfirm));
    }

    void OnConfirm()
    {
        eventObj.DoOption();
    }
}