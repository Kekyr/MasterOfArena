using System;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] private SFXSO _success;
    [SerializeField] private SFXSO _fail;

    private SFX _sfx;
    private PlayerDataSO _playerData;
    private Coins _coins;
    private ShopPopup _shopPopup;

    private void OnEnable()
    {
        if (_success == null)
        {
            throw new ArgumentNullException(nameof(_success));
        }

        if (_fail == null)
        {
            throw new ArgumentNullException(nameof(_fail));
        }

        _sfx = GetComponent<SFX>();

        _shopPopup.Selected += OnSelected;
    }

    private void OnDisable()
    {
        _shopPopup.Selected -= OnSelected;
    }

    public void Init(PlayerDataSO playerData, Coins coins, ShopPopup shopPopup)
    {
        if (playerData == null)
        {
            throw new ArgumentNullException(nameof(playerData));
        }

        if (coins == null)
        {
            throw new ArgumentNullException(nameof(coins));
        }

        if (shopPopup == null)
        {
            throw new ArgumentNullException(nameof(shopPopup));
        }

        _playerData = playerData;
        _coins = coins;
        _shopPopup = shopPopup;
        enabled = true;
    }

    public void TryBuy(SkinView skinView)
    {
        if (_coins.TryRemove(skinView.Data.Cost))
        {
            _sfx.Play(_fail);
            return;
        }

        _coins.Remove(skinView.Data.Cost);
        skinView.OnBuySuccess();
        _sfx.Play(_success);
    }

    private void OnSelected(int skinIndex)
    {
        _playerData.ChangeSkin(skinIndex);
    }
}