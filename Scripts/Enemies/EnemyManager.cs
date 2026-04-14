using Godot;
using System.Collections.Generic;

[GlobalClass]
public partial class EnemyManager : Node
{
    [ExportGroup("Spawn Settings")]
    [Export] Path3D _spawnBoundary;
    [Export] Vector2 _spawnHeightRange;
    [Export] PackedScene _enemyScene;

    [ExportGroup("References")]
    [Export] Node3D _enemyRoot;

    private float _boundaryLength;

    public override void _Ready()
    {
        _boundaryLength = _spawnBoundary.Curve.GetBakedLength();
    }

    public override void _Process(double delta)
    {
        if (_enemyRoot.GetChildCount() > 0)
        {
            return;
        }

        float randomOffset = (float)GD.RandRange(0f, _boundaryLength);
        Vector3 spawnPosition = _spawnBoundary.Curve.SampleBaked(randomOffset);
        spawnPosition.Y += (float)GD.RandRange(_spawnHeightRange.X, _spawnHeightRange.Y);
        SpawnEnemy(spawnPosition + _spawnBoundary.GlobalPosition);
    }

    private void SpawnEnemy(Vector3 position)
    {
        Enemy enemy = _enemyScene.Instantiate<Enemy>();
        _enemyRoot.AddChild(enemy);

        enemy.Position = position;
        enemy.Killed += OnEnemyKilled;

        enemy.LookAt(default);
        enemy.RotateX((float)GD.RandRange(0.5f, 1.5f));
        enemy.RotateY((float)GD.RandRange(-1f, 1f));
    }

    private void OnEnemyKilled(Enemy enemy)
    {
    }
}
