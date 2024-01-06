using System;
using System.Collections;
using UnityEngine;

public class AiAiming : Aiming
{
    private readonly float CircleNewScale = 0.02f;
    private readonly float AimNewScale = 0.05f;
    private readonly float Delay = 1f;

    private CubeSpawner _cubeSpawner;
    private WaitForSeconds _waitForSeconds;

    private void Start()
    {
        _waitForSeconds = new WaitForSeconds(Delay);
        OnCatch();
    }

    public void Init(CubeSpawner cubeSpawner)
    {
        if (cubeSpawner == null)
            throw new ArgumentNullException(nameof(cubeSpawner));

        _cubeSpawner = cubeSpawner;
        enabled = true;
    }

    private IEnumerator Aiming()
    {
        Circle.ChangeScale(CircleNewScale);
        yield return _waitForSeconds;

        Aim.ChangeScale(AimNewScale);
        yield return _waitForSeconds;

        Vector3 throwDirection = (_cubeSpawner.GetRandomCubePosition() - transform.position).normalized;
        RotateTo(throwDirection);
        InvokeAimed(throwDirection);
    }

    protected override void OnCatch()
    {
        StartCoroutine(Aiming());
    }
}