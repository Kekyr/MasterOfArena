using System;

public class PlayerInputRouter
{
    private readonly PlayerInput Input;
    private readonly Aiming Aiming;

    public PlayerInputRouter(Aiming aiming)
    {
        if (aiming == null)
            throw new ArgumentNullException(nameof(aiming));

        Input = new PlayerInput();
        Aiming = aiming;
    }

    public void OnEnable()
    {
        Input.Enable();
        Input.Player.Aiming.performed += ctx => Aiming.OnAimingStarted();
        Input.Player.Aiming.canceled += ctx => Aiming.OnAimingCanceled();
    }

    public void OnDisable()
    {
        Input.Player.Aiming.performed -= ctx => Aiming.OnAimingStarted();
        Input.Player.Aiming.canceled -= ctx => Aiming.OnAimingCanceled();
        Input.Disable();
    }
}