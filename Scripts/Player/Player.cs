using EventData;
using Godot;

[GlobalClass]
public partial class Player : RigidBody3D
{
    public float Health { get; private set; }
    public Vector2 LookInput;
    public Vector2 MoveInput;

    [Export] PlayerStats _stats;

    [ExportGroup("References")]
    [Export] Node3D _viewRoot;
    [Export] AnimationPlayer _animator;
    [Export] Camera3D _camera3D;
    [Export] RayCast3D _rayCast;
    [Export] Weapon _weapon;

    private float _targetSpeed;
    private float _targetLean;
    private float _targetViewPitch;

    public override void _EnterTree()
    {
        Game.Player = this;
    }

    public override void _ExitTree()
    {
        Game.Player = null;
    }

    public override void _Ready()
    {
        Input.MouseMode = Input.MouseModeEnum.Captured;
        _targetSpeed = _stats.RunSpeed;

        Health = _stats.MaxHealth;
        Events.Invoke(new PlayerHealthChanged
        {
            PrevHealth = 0,
            NewHealth = Health,
        });
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseMotion eventMouseMotion)
        {
            LookInput.X = -eventMouseMotion.Relative.X * Game.Settings.MouseSensitivity;
            LookInput.Y = -eventMouseMotion.Relative.Y * Game.Settings.MouseSensitivity;
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        HandleLook();
        HandleMove();
        HandleLean();
        HandleViewRotation();
        HandleFire();
        ResetInput();

        void HandleLook()
        {
            if (LookInput.Y == 0)
            {
                LookInput.Y = -Input.GetAxis("rs_up", "rs_down") * Game.Settings.StickSensitivity;
            }

            if (LookInput.X == 0)
            {
                LookInput.X = -Input.GetAxis("rs_left", "rs_right") * Game.Settings.StickSensitivity;
            }

            if (LookInput.Y != 0)
            {
                float pitchMin = Mathf.DegToRad(_stats.PitchMin);
                float pitchMax = Mathf.DegToRad(_stats.PitchMax);
                _targetViewPitch = Mathf.Clamp(_targetViewPitch + LookInput.Y, pitchMin, pitchMax);
            }

            if (LookInput.X != 0)
            {
                Rotate(Vector3.Up, LookInput.X);
            }
        }

        void HandleMove()
        {
            MoveInput.Y = Input.GetAxis("ls_up", "ls_down");
            MoveInput.X = Input.GetAxis("ls_left", "ls_right");

            Vector3 inputDir = new Vector3(MoveInput.X, 0, MoveInput.Y);
            float length = inputDir.Length();

            if (length > 1)
            {
                inputDir = inputDir.Normalized();
            }

            Vector3 direction = Transform.Basis * inputDir;
            ApplyForce(direction * _targetSpeed);
        }

        void HandleLean()
        {
            if (MoveInput.X != 0)
            {
                _targetLean = _stats.LeanAmount * -MoveInput.X;
            }
            else
            {
                _targetLean = 0;
            }
        }

        void HandleViewRotation()
        {
            float leanSpeed = _targetLean == 0 ? _stats.LeanSpeed * 1.5f : _stats.LeanSpeed;
            Vector3 viewRotation = _viewRoot.Rotation;
            viewRotation.X = _targetViewPitch;
            viewRotation.Z = Mathf.Lerp(viewRotation.Z, Mathf.DegToRad(_targetLean), Game.FixedDeltaTime * leanSpeed);
            _viewRoot.Rotation = viewRotation;
        }

        void HandleFire()
        {
            if (!Input.IsActionJustPressed("fire"))
            {
                return;
            }

            _rayCast.ForceRaycastUpdate();
            _weapon.Fire(_rayCast);
            _animator.Stop();
            _animator.Play("fire", -1);
        }

        void ResetInput()
        {
            LookInput = default;
            MoveInput = default;
        }
    }

    public bool IsDead()
    {
        return Health <= 0;
    }

    public void Damage(float amount)
    {
        if (IsDead())
        {
            return;
        }

        float prevHealth = Health;
        Health = Mathf.Max(0f, Health - amount);
        Events.Invoke(new PlayerHealthChanged
        {
            PrevHealth = prevHealth,
            NewHealth = Health,
        });

        if (IsDead())
        {
            Game.CallDeferred(() => GetTree().ReloadCurrentScene());
        }
    }
}
