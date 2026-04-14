using Godot;

[GlobalClass]
public partial class GameSettings : Resource
{
    [Export] public float MouseSensitivity = 0.002f;
    [Export] public float StickSensitivity = 0.05f;
}
