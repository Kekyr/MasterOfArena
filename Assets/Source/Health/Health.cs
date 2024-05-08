using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private uint _start;

    private float _current;

    private BombPlatform _platform;

    public event Action<float> HealthChanged;

    public event Action Died;

    public bool IsDead => _current <= 0;

    private void OnEnable()
    {
        _current = _start;
        HealthChanged?.Invoke(_current);

        _platform.Attacked += OnAttacked;
    }

    private void OnDisable()
    {
        _platform.Attacked -= OnAttacked;
    }

    public void Init(BombPlatform platform)
    {
        if (platform == null)
        {
            throw new ArgumentNullException(nameof(platform));
        }

        _platform = platform;
        enabled = true;
    }

    private void OnAttacked(uint damage)
    {
        _current -= damage;

        HealthChanged?.Invoke(_current);

        if (IsDead)
        {
            Died?.Invoke();
        }
    }
}