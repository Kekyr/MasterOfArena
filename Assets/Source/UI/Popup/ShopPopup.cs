using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShopPopup : MainPopup
{
    private GridLayoutGroup _content;
    private List<SkinView> _skinViewes;
    private SkinSO[] _skins;
    private Shop _shop;
    private SkinRewardedAd _skinRewardedAd;

    private SkinView _startSkinView;
    private SkinView _currentSkinView;

    private Health _playerHealth;
    private Health _enemyHealth;

    private bool _isLevelEnded;

    public event Action<Player> Selected;

    private void Awake()
    {
        _content = GetComponentInChildren<GridLayoutGroup>();
        _skinViewes = new List<SkinView>();

        foreach (SkinSO skin in _skins)
        {
            SkinView view = Instantiate(skin.View, _content.transform);
            view.Selected += OnSelected;
            view.TryBuy += OnTryBuy;

            if (skin.Status == State.Selected)
            {
                _startSkinView = view;
            }

            view.Init(skin);
            _skinViewes.Add(view);
        }
    }

    private void OnDestroy()
    {
        foreach (SkinView skinView in _skinViewes)
        {
            skinView.Selected -= OnSelected;
            skinView.TryBuy -= OnTryBuy;
        }

        _playerHealth.Died -= OnDied;
        _enemyHealth.Died -= OnDied;
    }

    public void Init(Health playerHealth, Health enemyHealth)
    {
        if (playerHealth == null)
        {
            throw new ArgumentNullException(nameof(playerHealth));
        }

        if (enemyHealth == null)
        {
            throw new ArgumentNullException(nameof(enemyHealth));
        }

        _playerHealth = playerHealth;
        _enemyHealth = enemyHealth;

        _playerHealth.Died += OnDied;
        _enemyHealth.Died += OnDied;
    }

    public void Init(SkinSO[] skins, Shop shop, SkinRewardedAd skinRewardedAd)
    {
        if (skins.Length == 0)
        {
            throw new ArgumentOutOfRangeException(nameof(skins));
        }

        if (shop == null)
        {
            throw new ArgumentNullException(nameof(shop));
        }

        if (skinRewardedAd == null)
        {
            throw new ArgumentNullException(nameof(skinRewardedAd));
        }

        _skins = skins;
        _shop = shop;
        _skinRewardedAd = skinRewardedAd;
        enabled = true;
    }

    protected override void Close()
    {
        base.Close();

        if (_currentSkinView != _startSkinView && _isLevelEnded == false)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void OnDied()
    {
        _isLevelEnded = true;
    }

    private void OnSelected(SkinView skinView)
    {
        if (_currentSkinView != null)
        {
            _currentSkinView.Deselect();
        }

        skinView.Select();
        _currentSkinView = skinView;
        Selected?.Invoke(_currentSkinView.Data.Prefab);
    }

    private void OnTryBuy(SkinView skinView)
    {
        if (skinView.Data.Ad == true)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            _skinRewardedAd.Show(skinView);
#endif
        }
        else
        {
            _shop.TryBuy(skinView);
        }
    }
}