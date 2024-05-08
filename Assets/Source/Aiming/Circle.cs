using System;

public class Circle : AimElement
{
    private Projectile[] _projectiles;

    protected override void OnEnable()
    {
        base.OnEnable();

        foreach (Projectile projectile in _projectiles)
        {
            projectile.Catched += IncreaseScale;
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        foreach (Projectile projectile in _projectiles)
        {
            projectile.Catched -= IncreaseScale;
        }
    }

    public void Init(Projectile[] projectiles)
    {
        if (projectiles == null)
        {
            throw new ArgumentNullException(nameof(projectiles));
        }

        _projectiles = projectiles;
    }
}