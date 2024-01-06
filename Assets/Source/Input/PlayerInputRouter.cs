using UnityEngine.InputSystem;

public class PlayerInputRouter
{
    private PlayerInput _input;

    public InputAction Aiming => _input.Player.Aiming;

    public PlayerInputRouter()
    {
        _input = new PlayerInput();
    }

    public void OnEnable()
    {
        _input.Enable();
    }

    public void OnDisable()
    {
        _input.Disable();
    }
}