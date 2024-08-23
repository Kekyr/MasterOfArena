using System;
using UnityEngine;

public class ShopButton : MainButton
{
    [SerializeField] private ShopPopup _popup;

    protected override void OnEnable()
    {
        base.OnEnable();

        if (_popup == null)
        {
            throw new ArgumentNullException(nameof(_popup));
        }
    }

    protected override void OnClick()
    {
        _popup.gameObject.SetActive(true);
    }
}