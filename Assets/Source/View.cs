using System;
using TMPro;
using UnityEngine;

public class View : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textMesh;

    private void OnEnable()
    {
        if (_textMesh == null)
            throw new ArgumentNullException(nameof(_textMesh));
    }

    public void Init(string startText)
    {
        ChangeText(startText);
    }

    protected void ChangeText(string newText)
    {
        _textMesh.text += newText;
    }
}