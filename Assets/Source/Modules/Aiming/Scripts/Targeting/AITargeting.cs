using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITargeting : Targeting
{
    private readonly float _delay = 2f;

    private IReadOnlyCollection<Projectile> _projectiles;
    private CubeSpawner _cubeSpawner;
    private WaitForSeconds _waitForSeconds;

    protected override void OnEnable()
    {
        base.OnEnable();

        foreach (Projectile projectile in _projectiles)
        {
            projectile.Catched += OnCatch;
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        foreach (Projectile projectile in _projectiles)
        {
            projectile.Catched -= OnCatch;
        }
    }

    private void Start()
    {
        _waitForSeconds = new WaitForSeconds(_delay);
        OnCatch();
    }

    public void Init(CubeSpawner cubeSpawner, IReadOnlyCollection<Projectile> projectiles)
    {
        _cubeSpawner = cubeSpawner;
        _projectiles = projectiles;
    }

    private IEnumerator FindingTarget()
    {
        InvokeAiming();
        yield return _waitForSeconds;

        Vector3 throwDirection = (_cubeSpawner.GetRandomCubePosition() - transform.position).normalized;
        RotateTo(throwDirection);
        InvokeAimed(throwDirection);
    }

    private void OnCatch()
    {
        StartCoroutine(FindingTarget());
    }
}