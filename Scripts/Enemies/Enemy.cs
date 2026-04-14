using Godot;
using System;

[GlobalClass]
public partial class Enemy : Area3D
{
    public event Action<Enemy> Killed;

    [ExportGroup("Health")]
    [Export] float _maxHealth = 1.0f;
    [Export] float _contactDamage = 10.0f;

    [ExportGroup("Movement")]
    [Export] Curve _turnForce;
    [Export] float _forwardForce = 2.0f;
    [Export] float _maxSpeed = 0.15f;

    [ExportGroup("References")]
    [Export] Area3D _hurtBoxArea;

    private Vector3 _velocity;
    private float _health;

    public override void _Ready()
    {
        _health = _maxHealth;
        _hurtBoxArea.BodyEntered += OnBodyEntered;
    }

    public bool IsDead()
    {
        return _health <= 0;
    }

    public void Damage(float amount)
    {
        if (IsDead())
        {
            return;
        }

        _health = Mathf.Max(0f, _health - amount);

        if (IsDead())
        {
            Kill();
        }
    }

    public void Kill()
    {
        Killed?.Invoke(this);
        QueueFree();
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector3 target = Game.Player.Position;
        _velocity += Game.FixedDeltaTime * _forwardForce * -Basis.Z;
        _velocity += Game.FixedDeltaTime * _turnForce.Sample(Position.DistanceTo(target)) * Position.DirectionTo(target);

        if (Position.Y < 1.25f)
        {
            _velocity += Game.FixedDeltaTime * (1.25f - Position.Y) * Vector3.Up;
        }

        if (_velocity.LengthSquared() > _maxSpeed * _maxSpeed)
        {
            _velocity = _velocity.Normalized() * _maxSpeed;
        }

        LookAt(Position + _velocity);
        Position += _velocity;
    }

    private void OnBodyEntered(Node3D body)
    {
        if (body is not Player player)
        {
            return;
        }

        player.Damage(_contactDamage);
        Kill();
    }
}
