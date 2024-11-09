using System;
using Audio;
using Money;
using PlayerBase;
using SaveSystem;
using UnityEngine;

namespace ShopSystem
{
    public class Shop : MonoBehaviour
    {
        [SerializeField] private SFXSO _success;
        [SerializeField] private SFXSO _fail;

        private SFX _sfx;
        private PlayerDataSO _playerData;
        private Coins _coins;
        private ShopPopup _shopPopup;
        private SaveLoader _saveLoader;

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

        public void Init(PlayerDataSO playerData, Coins coins, ShopPopup shopPopup, SaveLoader saveLoader)
        {
            _playerData = playerData;
            _coins = coins;
            _shopPopup = shopPopup;
            _saveLoader = saveLoader;
            enabled = true;
        }

        public void TryBuy(SkinView skinView)
        {
            if (_coins.TryDecrease(skinView.Data.Cost))
            {
                _sfx.Play(_fail);
                return;
            }

            _coins.Decrease(skinView.Data.Cost);
            skinView.OnBuySuccess();
            _saveLoader.Save();
            _sfx.Play(_success);
        }

        private void OnSelected(int skinIndex)
        {
            _playerData.ChangeSkin(skinIndex);
            _saveLoader.Save();
        }
    }
}