using System;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] private SkinSO[] _skins;

    private PlayerDataSO _playerData;
    private ShopPopup _shopPopup;

    private void OnEnable()
    {
        if (_skins.Length == 0)
        {
            throw new ArgumentOutOfRangeException(nameof(_skins));
        }

        _shopPopup.Init(_skins);
    }

    public void Init(PlayerDataSO playerData, ShopPopup shopPopup)
    {
        if (playerData == null)
        {
            throw new ArgumentNullException(nameof(playerData));
        }

        if (shopPopup == null)
        {
            throw new ArgumentNullException(nameof(shopPopup));
        }

        _playerData = playerData;
        _shopPopup = shopPopup;
        enabled = true;
    }
}