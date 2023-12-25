using System;
using TMPro;
using UnityEngine;

public class CubeView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textMesh;

    private string _text;

    private void OnEnable()
    {
        if (_textMesh == null)
            throw new ArgumentNullException(nameof(_textMesh));

        _textMesh.text = _text;
    }

    public void Init(string text)
    {
        if (text == null)
            throw new ArgumentNullException(nameof(text));

        _text = text;
    }
}