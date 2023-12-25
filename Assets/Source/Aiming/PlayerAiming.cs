using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAiming : Aiming
{
    private readonly float DistanceFromCamera = 19.8f;

    [SerializeField] private float _multiplierZ;

    private bool canAim;
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

    public void Init(Projectile[] projectiles)
    {
        int maxLength = 2;

        if (projectiles.Length == 0 || projectiles.Length > maxLength)
            throw new ArgumentOutOfRangeException(nameof(projectiles));

        _projectiles = projectiles;
        enabled = true;
    }

    private IEnumerator Aiming()
    {
        Vector3 throwDirection = Vector3.zero;

        while (canAim)
        {
            throwDirection = TakeAim();
            RotateTo(throwDirection);
            yield return null;
        }

        InvokeAimed(throwDirection);
    }

    private Vector3 TakeAim()
    {
        Vector3 pointerScreenPosition = Pointer.current.position.ReadValue();
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(pointerScreenPosition.x, pointerScreenPosition.y, DistanceFromCamera));
        mouseWorldPosition = new Vector3(mouseWorldPosition.x, transform.position.y, mouseWorldPosition.z * _multiplierZ);
        Vector3 throwDirection = -(mouseWorldPosition - transform.position).normalized;
        return throwDirection;
    }

    public void OnAimingStarted()
    {
        if (Catcher.CurrentProjectile.IsFlying == false)
        {
            Aim.ChangeScale(0.05f);
            canAim = true;
            StartCoroutine(Aiming());
        }
    }

    public void OnAimingCanceled()
    {
        canAim = false;
    }

    private void OnCatch()
    {
        Circle.ChangeScale(0.02f);
    }
}