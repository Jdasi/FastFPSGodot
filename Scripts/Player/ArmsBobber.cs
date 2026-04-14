using Godot;

public partial class ArmsBobber : Node3D
{
    [ExportGroup("Settings")]
    [Export] float _lookLagSpeed = 6.0f;
    [Export] float _lookLagStrength = 1.0f;
    [Export] float _maxLookRotationLag = 0.2f;
    [Export] float _maxLookPositionLag = 0.1f;

    [ExportGroup("References")]
    [Export] Node3D _camera;
    [Export] Player _player;

    private Vector3 _startPosition;
    private Vector3 _prevCameraRotation;

    public override void _Ready()
    {
        _startPosition = Position;
        _prevCameraRotation = _camera.GlobalRotation;
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector3 currentRotation = _camera.GlobalRotation;
        Vector3 deltaRot = currentRotation - _prevCameraRotation;

        deltaRot.X = Mathf.Wrap(deltaRot.X, -Mathf.Pi, Mathf.Pi);
        deltaRot.Y = Mathf.Wrap(deltaRot.Y, -Mathf.Pi, Mathf.Pi);
        deltaRot.Z = 0;

        Vector3 targetRotationLag = deltaRot * _lookLagStrength;
        targetRotationLag.X = Mathf.Clamp(targetRotationLag.X, -_maxLookRotationLag, _maxLookRotationLag);
        targetRotationLag.Y = Mathf.Clamp(targetRotationLag.Y, -_maxLookRotationLag, _maxLookRotationLag);

        Vector3 rotation = Rotation.Lerp(targetRotationLag, Game.FixedDeltaTime * _lookLagSpeed);
        Vector3 positionLag = rotation * _maxLookPositionLag;

        Rotation = rotation;
        Position = new Vector3(_startPosition.X + positionLag.Y, _startPosition.Y - positionLag.X, _startPosition.Z);

        _prevCameraRotation = currentRotation;
    }
}
