using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputRouter : MonoBehaviour
{
    private PlayerInput _input;
    private Health _health;
    private Health _enemyHealth;

    public InputAction Aiming => _input.Player.Aiming;

    private void Awake()
    {
        _input = new PlayerInput();
    }

    private void OnEnable()
    {
        _input.Enable();
        _health.Died += OnDead;
        _enemyHealth.Died += OnDead;
    }

    private void OnDisable()
    {
        _input.Disable();
        _health.Died -= OnDead;
        _enemyHealth.Died -= OnDead;
    }

    public void Init(Health health, Health enemyHealth)
    {
        if (health == null)
        {
            throw new ArgumentNullException(nameof(health));
        }

        if (enemyHealth == null)
        {
            throw new ArgumentNullException(nameof(enemyHealth));
        }

        _health = health;
        _enemyHealth = enemyHealth;
        enabled = true;
    }

    private void OnDead()
    {
        _input.Disable();
    }
}