using System;
using System.Collections;
using UnityEngine;

public class AiAiming : Aiming
{
    private CubeSpawner _cubeSpawner;
    private Projectile[] _projectiles;
    private WaitForSeconds _waitForSeconds;

    protected override void OnEnable()
    {
        base.OnEnable();

        _waitForSeconds = new WaitForSeconds(1f);

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

    public IEnumerator Aiming()
    {
        Circle.ChangeScale(0.02f);
        yield return _waitForSeconds;
        Aim.ChangeScale(0.05f);
        yield return _waitForSeconds;
        Vector3 throwDirection = (_cubeSpawner.GetRandomCubePosition() - transform.position).normalized;
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

    private void OnCatch()
    {
        StartCoroutine(Aiming());
    }
}