using System;
using UnityEngine;
using UnityEngine.UI;

public abstract class Popup : MonoBehaviour
{
    [SerializeField] private Button _nextButton;

    protected virtual void OnEnable()
    {
        if (_nextButton == null)
        {
            throw new ArgumentNullException(nameof(_nextButton));
        }

        _nextButton.onClick.AddListener(Next);
    }

    protected virtual void OnDisable()
    {
        _nextButton.onClick.RemoveListener(Next);
    }

    protected abstract void Next();
}