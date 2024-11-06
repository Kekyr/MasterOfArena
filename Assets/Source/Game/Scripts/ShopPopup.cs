using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShopPopup : MainPopup
{
    private GridLayoutGroup _content;
    private List<SkinView> _skinViewes;
    private SkinDataSO[] _skinsData;
    private Shop _shop;
    private RewardedAd _rewardedAd;

    private SkinView _startSkinView;
    private SkinView _currentSkinView;

    private Health _playerHealth;
    private Health _enemyHealth;

    private bool _isLevelEnded;

    public event Action<int> Selected;

    private void Awake()
    {
        _content = GetComponentInChildren<GridLayoutGroup>();
        _skinViewes = new List<SkinView>();

        foreach (SkinDataSO skinData in _skinsData)
        {
            SkinView view = Instantiate(skinData.View, _content.transform);
            view.Selected += OnSelected;
            view.TryBuy += OnTryBuy;

            if (skinData.Status == State.Selected)
            {
                _startSkinView = view;
            }

            view.Init(skinData);
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
        _playerHealth = playerHealth;
        _enemyHealth = enemyHealth;

        _playerHealth.Died += OnDied;
        _enemyHealth.Died += OnDied;
    }

    public void Init(SkinDataSO[] skinsData, Shop shop, RewardedAd rewardedAd)
    {
        _skinsData = skinsData;
        _shop = shop;
        _rewardedAd = rewardedAd;
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
        Selected?.Invoke(_currentSkinView.Data.PrefabIndex);
    }

    private void OnTryBuy(SkinView skinView)
    {
        if (skinView.Data.Ad == true)
        {
            _rewardedAd.Show(skinView);
        }
        else
        {
            _shop.TryBuy(skinView);
        }
    }
}