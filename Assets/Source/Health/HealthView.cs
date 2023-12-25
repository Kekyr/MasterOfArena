using System;
using TMPro;
using UnityEngine;

public class HealthView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textMesh;

    private string _text;

    private void OnEnable()
    {
        if (_textMesh == null)
            throw new ArgumentNullException(nameof(_textMesh));

        _text = _textMesh.text;
    }

    public void OnHealthChanged(float health)
    {
        _textMesh.text = $"{_text} {health.ToString()}";
    }
}