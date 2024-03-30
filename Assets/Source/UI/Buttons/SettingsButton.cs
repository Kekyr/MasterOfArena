using System;
using UnityEngine;
using UnityEngine.UI;

public class SettingsButton : MainButton
{
    [SerializeField] private Button _button;
    [SerializeField] private SettingsPopup _popup;

    protected override void OnEnable()
    {
        base.OnEnable();

        if (_button == null)
            throw new ArgumentNullException(nameof(_button));

        if (_popup == null)
            throw new ArgumentNullException(nameof(_popup));

        _button.onClick.AddListener(OnClick);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnClick);
    }

    private void OnClick()
    {
        _popup.gameObject.SetActive(true);
    }
}