using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private uint _start;

    private float _current;

    public event Action<float> HealthChanged;

    public event Action Died;

    public bool IsDead => _current <= 0;

    private void OnEnable()
    {
        _current = _start;
        HealthChanged?.Invoke(_current);
    }

    public void Init()
    {
        enabled = true;
    }

    public void ApplyDamage(uint damage)
    {
        _current -= damage;

        HealthChanged?.Invoke(_current);

        if (IsDead)
            Died?.Invoke();
    }
}