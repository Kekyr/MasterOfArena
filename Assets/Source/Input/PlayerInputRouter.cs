using System;

public class PlayerInputRouter
{
    private readonly PlayerInput _input;
    private readonly Aiming _aimPresenter;

    public PlayerInputRouter(Aiming aimPresenter)
    {
        if (aimPresenter == null)
            throw new ArgumentNullException(nameof(aimPresenter));

        _input = new PlayerInput();
        _aimPresenter = aimPresenter;
    }

    public void OnEnable()
    {
        _input.Enable();
        _input.Player.Aiming.started += ctx => _aimPresenter.OnAimingStarted();
        _input.Player.Aiming.canceled += ctx => _aimPresenter.OnAimingCanceled();
    }

    public void OnDisable()
    {
        _input.Player.Aiming.started -= ctx => _aimPresenter.OnAimingStarted();
        _input.Player.Aiming.canceled -= ctx => _aimPresenter.OnAimingCanceled();
        _input.Disable();
    }
}