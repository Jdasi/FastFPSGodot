using Godot;

[GlobalClass]
public partial class Weapon : Node3D
{
    [Export] int _damage;
    [Export] MuzzleFlash _muzzleFlash;

    public void Fire(RayCast3D rayCast)
    {
        DealDamage(rayCast);
        _muzzleFlash.Trigger();
    }

    private void DealDamage(RayCast3D rayCast)
    {
        if (rayCast.GetCollider() is not Enemy enemy)
        {
            return;
        }

        enemy.Damage(_damage);
    }
}
