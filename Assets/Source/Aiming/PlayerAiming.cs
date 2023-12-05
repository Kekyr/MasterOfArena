using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAiming : Aiming
{
    private readonly float DistanceFromCamera = 19.8f;
    private bool canAim;

    public void OnAimingStarted()
    {
        if (Catcher.CurrentProjectile.IsFlying == false)
        {
            StartCoroutine(LerpAlpha(1));
            canAim = true;
            StartCoroutine(Aim());
        }
    }

    public void OnAimingCanceled()
    {
        canAim = false;
    }

    private IEnumerator Aim()
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

    protected override Vector3 TakeAim()
    {
        Vector3 mouseScreenPosition = Mouse.current.position.ReadValue();
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(mouseScreenPosition.x, mouseScreenPosition.y, DistanceFromCamera));

        mousePosition = new Vector3(mousePosition.x, transform.position.y, mousePosition.z);

        Vector3 throwDirection = -(mousePosition - transform.position).normalized;

        return throwDirection;
    }
}