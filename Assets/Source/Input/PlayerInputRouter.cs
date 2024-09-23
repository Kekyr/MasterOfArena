using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputRouter : MonoBehaviour
{
    private PlayerInput _input;
    private Health _health;
    private Health _enemyHealth;
    private Sequence _sequence;

    public InputAction Aiming => _input.Player.Aiming;

    private void OnDisable()
    {
        _input.Disable();
        _health.Died -= OnDead;
        _enemyHealth.Died -= OnDead;
    }

    public void Init(Health health, Health enemyHealth, Sequence sequence)
    {
        _health = health;
        _enemyHealth = enemyHealth;
        _sequence = sequence;

        _health.Died += OnDead;
        _enemyHealth.Died += OnDead;

        _input = new PlayerInput();
        _sequence.OnComplete(OnComplete);
    }

    private void OnDead()
    {
        _input.Disable();
    }

    private void OnComplete()
    {
        _input.Enable();
    }
}