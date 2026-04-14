using Godot;

[GlobalClass]
public partial class TestBed : Node
{
    [Export] PackedScene _enemyScene;

    public override void _UnhandledKeyInput(InputEvent @event)
    {
        if (@event is InputEventKey key
            && key.IsReleased())
        {
            switch (key.Keycode)
            {
                case Key.E:
                {
                    Vector3 pos = (Vector3.Up * 5f) + (Game.Player.Position - Game.Player.Basis.Z * 20f);
                    SpawnEnemy(pos);
                } break;
            }
        }
    }

    private void SpawnEnemy(Vector3 position)
    {
        Enemy enemy = _enemyScene.Instantiate<Enemy>();
        AddChild(enemy);
        enemy.LookAtFromPosition(position, Game.Player.Position);
    }
}
