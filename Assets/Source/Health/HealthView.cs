using System;
using TMPro;
using UnityEngine;

public class HealthView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textMesh;

    private Health _health;
    private string _text;

    private void OnEnable()
    {
        if (_textMesh == null)
            throw new ArgumentNullException(nameof(_textMesh));

        _text = _textMesh.text;

        _health.HealthChanged += OnHealthChanged;
    }

    private void OnDisable()
    {
        _health.HealthChanged -= OnHealthChanged;
    }

    public void Init(Health health)
    {
        if (health == null)
            throw new ArgumentNullException(nameof(health));

        _health = health;
        enabled = true;
    }

    private void OnHealthChanged(float health)
    {
        _textMesh.text = $"{_text}  {health.ToString()}";
    }
}