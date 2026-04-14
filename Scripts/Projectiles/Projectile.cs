using Godot;

[GlobalClass]
public partial class Projectile : RigidBody3D
{
    private const float MAX_LIFETIME = 15f;

    [Export] float _speed = 10.0f;

    private Node _owner;
    private float _lifetime;

    public void Init(Node owner)
    {
        BodyEntered += OnBodyEntered;
        LinearVelocity = -Basis.Z * _speed;
        AddCollisionExceptionWith(owner);
    }

    public override void _PhysicsProcess(double delta)
    {
        _lifetime += Game.FixedDeltaTime;

        if (_lifetime >= MAX_LIFETIME)
        {
            QueueFree();
        }
    }

    private void OnBodyEntered(Node body)
    {
        QueueFree();
    }
}
