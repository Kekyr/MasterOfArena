using System;
using System.Collections;
using UnityEngine;

public class AiAiming : Aiming
{
    private CubeSpawner _cubeSpawner;
    private Projectile[] _projectiles;

    protected override void OnEnable()
    {
        base.OnEnable();

        foreach (Projectile projectile in _projectiles)
            projectile.Catched += OnCatch;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        foreach (Projectile projectile in _projectiles)
            projectile.Catched -= OnCatch;
    }

    private void Start()
    {
        OnCatch();
    }

    public IEnumerator Aim()
    {
        yield return StartCoroutine(LerpAlpha(1));
        Vector3 throwDirection = TakeAim();
        RotateTo(throwDirection);
        InvokeAimed(throwDirection);
    }

    public void Init(CubeSpawner cubeSpawner, Projectile[] projectiles)
    {
        int maxLength = 2;

        if (cubeSpawner == null)
            throw new ArgumentNullException(nameof(cubeSpawner));

        if (projectiles.Length == 0 || projectiles.Length > maxLength)
            throw new ArgumentOutOfRangeException(nameof(projectiles));

        _cubeSpawner = cubeSpawner;
        _projectiles = projectiles;
        enabled = true;
    }

    protected override Vector3 TakeAim()
    {
        Vector3 throwDirection = (_cubeSpawner.GetRandomCubePosition() - transform.position).normalized;
        return throwDirection;
    }

    private void OnCatch()
    {
        StartCoroutine(Aim());
    }
}