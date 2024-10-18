using System;
using UnityEngine;
using YG;

[RequireComponent(typeof(SFX))]
public class RewardedAd : MonoBehaviour
{
    [SerializeField] private SFXSO _success;

    private SaveLoader _saveLoader;
    private SFX _sfx;
    private Music _music;
    private SkinView _currentSkinView;

    private PlayerDataSO _playerData;
    private Coins _coins;
    private int _id;

    public event Action Rewarded;

    private void OnEnable()
    {
        if (_success == null)
        {
            throw new ArgumentNullException(nameof(_success));
        }

        _sfx = GetComponent<SFX>();

        YandexGame.RewardVideoEvent += OnRewardCallback;
        YandexGame.OpenVideoEvent += OnOpenCallback;
        YandexGame.CloseVideoEvent += OnCloseCallback;
    }

    private void OnDisable()
    {
        YandexGame.RewardVideoEvent -= OnRewardCallback;
        YandexGame.OpenVideoEvent -= OnOpenCallback;
        YandexGame.CloseVideoEvent -= OnCloseCallback;
    }

    public void Init(SaveLoader saveLoader, Music music, PlayerDataSO playerData, Coins coins)
    {
        _saveLoader = saveLoader;
        _music = music;
        _playerData = playerData;
        _coins = coins;
    }

    public void Show(SkinView skinView)
    {
        _currentSkinView = skinView;
        Show((int)Reward.Skin);
    }

    public void Show(int id)
    {
        _id = id;
        YandexGame.RewVideoShow(id);
    }

    private void OnOpenCallback()
    {
        if (_id == (int)Reward.Resources)
        {
            Debug.Log("I'm here timescale0");
            Time.timeScale = 0;
        }
        else if (_id == (int)Reward.Skin)
        {
            Debug.Log("I'm here pause");
            _music.Pause();
        }
    }

    private void OnRewardCallback(int id)
    {
        if (id == (int)Reward.Resources)
        {
            _coins.Increase();
            _playerData.AddScore();
            Rewarded?.Invoke();
        }
        else if (id == (int)Reward.Skin)
        {
            _currentSkinView.OnBuySuccess();
        }

        _sfx.Play(_success);
        _saveLoader.Save();
    }

    private void OnCloseCallback()
    {
        if (_id == (int)Reward.Resources)
        {
            Debug.Log("I'm here timescale1");
            Time.timeScale = 1;
        }
        else if (_id == (int)Reward.Skin)
        {
            Debug.Log("I'm here continue");
            _music.Continue();
        }
    }
}