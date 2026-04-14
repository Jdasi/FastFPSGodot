using Godot;
using System;

public partial class Game : Node
{
    public static float DeltaTime { get; private set; }
    public static float FixedDeltaTime { get; private set; }
    public static Player Player;
    public static GameSettings Settings;

    private static Action _deferredCalls;

    public static void CallDeferred(Action action)
    {
        _deferredCalls += action;
    }

    public override void _EnterTree()
    {
        Settings = ResourceLoader.Load<GameSettings>("res://Resources/GameSettings.tres");
    }

    public override void _PhysicsProcess(double delta)
    {
        FixedDeltaTime = (float)delta;

        if (Input.IsActionJustPressed("esc"))
        {
            GetTree().Root.PropagateNotification((int)NotificationWMCloseRequest);
        }

        if (_deferredCalls != null)
        {
            _deferredCalls.Invoke();
            _deferredCalls = null;
        }
    }

    public override void _Process(double delta)
    {
        DeltaTime = (float)delta;
    }

    public override void _Notification(int what)
    {
        if (what == NotificationWMCloseRequest)
        {
            GetTree().Quit();
        }
    }

#if DEBUG
    public override void _UnhandledKeyInput(InputEvent @event)
    {
        if (@event is InputEventKey key
            && key.IsReleased())
        {
            switch (key.Keycode)
            {
                case Key.F1:
                {
                    Input.MouseMode = Input.MouseMode == Input.MouseModeEnum.Captured
                        ? Input.MouseModeEnum.Visible : Input.MouseModeEnum.Captured;
                } break;
            }
        }
    }
#endif
}
