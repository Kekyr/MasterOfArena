using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private uint _start;

    private HealthView _view;
    private float _current;

    public bool IsDead => _current <= 0;

    public event Action<float> HealthChanged;
    public event Action Died;

    private void OnEnable()
    {
        _view.Init(_start.ToString());
        HealthChanged += _view.OnHealthChanged;
    }

    private void OnDisable()
    {
        HealthChanged -= _view.OnHealthChanged;
    }

    public void Init(HealthView view)
    {
        if (view == null)
            throw new ArgumentNullException(nameof(view));

        _view = view;
        enabled = true;
    }

    public void ApplyDamage(int damage)
    {
        if (damage < 0)
            throw new ArgumentOutOfRangeException(nameof(damage));

        _current -= damage;

        HealthChanged?.Invoke(_current);

        if (IsDead)
            Died?.Invoke();
    }
}
