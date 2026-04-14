using EventData;
using Godot;

[GlobalClass]
public partial class HUD : CanvasLayer
{
    [ExportGroup("References")]
    [Export] Label _healthLabel;

    public override void _Ready()
    {
        if (Game.Player != null)
        {
            _healthLabel.Text = $"{(int)Game.Player.Health}";
        }

        Events.Subscribe<PlayerHealthChanged>(OnPlayerHealthChanged);
    }

    public override void _ExitTree()
    {
        Events.Unsubscribe<PlayerHealthChanged>(OnPlayerHealthChanged);
    }

    private void OnPlayerHealthChanged(PlayerHealthChanged data)
    {
        _healthLabel.Text = $"{(int)data.NewHealth}";
    }
}
