using Godot;

[GlobalClass]
public partial class PlayerStats : Resource
{
    [ExportGroup("Aiming")]
    [Export] public float PitchMin = -85.0f;
    [Export] public float PitchMax = 85.0f;

    [ExportGroup("Movement")]
    [Export] public float RunSpeed = 5.0f;
    [Export] public float SprintSpeed = 9.0f;
    [Export] public float CrouchSpeed = 2.0f;
    [Export] public float JumpVelocity = 3.5f;
    [Export] public float Acceleration = 0.1f;
    [Export] public float Deceleration = 0.5f;

    [ExportGroup("Leaning")]
    [Export] public float LeanAmount = 1.0f;
    [Export] public float LeanSpeed = 12.0f;

    [ExportGroup("Health")]
    [Export] public float MaxHealth = 100.0f;
}
