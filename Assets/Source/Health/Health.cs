using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private uint _start;

    private HealthView _view;
    private float _current;

    public event Action<float> HealthChanged;

    public event Action Died;

    public bool IsDead => _current <= 0;

    private void OnEnable()
    {
        _current = _start;
        _view.OnHealthChanged(_current);
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

    public void ApplyDamage(uint damage)
    {
        _current -= damage;

        HealthChanged?.Invoke(_current);

        if (IsDead)
            Died?.Invoke();
    }
}
