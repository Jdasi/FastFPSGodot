using Godot;

[GlobalClass]
public partial class MuzzleFlash : Node3D
{
    [Export] float _duration = 0.1f;
    [Export] OmniLight3D _light;
    [Export] GpuParticles3D _planes;
    [Export] GpuParticles3D _cone;
    [Export] GpuParticles3D _flash;

    private float _lifetime;

    public override void _Process(double delta)
    {
        if (_lifetime <= 0)
        {
            return;
        }

        _lifetime -= Game.DeltaTime;

        if (_lifetime > 0)
        {
            return;
        }

        SetEffectsEnabled(false);
    }

    public void Trigger()
    {
        SetEffectsEnabled(false);
        SetEffectsEnabled(true);
        _lifetime = _duration;
    }

    private void SetEffectsEnabled(bool enabled)
    {
        _light.Visible = enabled;
        _planes.Emitting = enabled;
        _cone.Emitting = enabled;
        _flash.Emitting = enabled;
    }
}
