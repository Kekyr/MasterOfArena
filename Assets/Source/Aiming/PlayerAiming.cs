using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAiming : Aiming
{
    private readonly float DistanceFromCamera = 19.8f;
    private readonly float NewAimScale = 0.05f;
    private readonly float NewCircleScale = 0.02f;

    [SerializeField] private float _multiplierZ;

    private PlayerInputRouter _inputRouter;

    private bool _canAim;

    protected override void OnEnable()
    {
        base.OnEnable();
        _inputRouter.Aiming.performed += ctx => OnAimingStarted();
        _inputRouter.Aiming.canceled += ctx => OnAimingCanceled();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _inputRouter.Aiming.performed -= ctx => OnAimingStarted();
        _inputRouter.Aiming.canceled -= ctx => OnAimingCanceled();
    }

    public void Init(PlayerInputRouter playerInputRouter)
    {
        if (playerInputRouter == null)
            throw new ArgumentNullException(nameof(playerInputRouter));

        _inputRouter = playerInputRouter;
        enabled = true;
    }

    private IEnumerator Aiming()
    {
        Vector3 throwDirection = Vector3.zero;

        while (_canAim)
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
        Vector3 mouseWorldPosition =
            Camera.main.ScreenToWorldPoint(new Vector3(pointerScreenPosition.x, pointerScreenPosition.y,
                DistanceFromCamera));
        mouseWorldPosition =
            new Vector3(mouseWorldPosition.x, transform.position.y, mouseWorldPosition.z * _multiplierZ);
        Vector3 throwDirection = -(mouseWorldPosition - transform.position).normalized;
        return throwDirection;
    }

    protected override void OnCatch()
    {
        Circle.ChangeScale(NewCircleScale);
    }

    protected override void OnDead()
    {
        enabled = false;
        Circle.ChangeScale(0f);
    }

    private void OnAimingStarted()
    {
        if (Catcher.CurrentProjectile.IsFlying == false)
        {
            Aim.ChangeScale(NewAimScale);
            _canAim = true;
            StartCoroutine(Aiming());
        }
    }

    private void OnAimingCanceled()
    {
        _canAim = false;
    }
}