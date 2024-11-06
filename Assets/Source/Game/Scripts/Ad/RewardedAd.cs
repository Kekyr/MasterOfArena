using System;
using Audio;
using LeaderboardBase;
using UnityEngine;
using YG;

[RequireComponent(typeof(SFX))]
public class RewardedAd : MonoBehaviour, IRewarder
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
        switch (_id)
        {
            case (int)Reward.Resources:
                Time.timeScale = 0;
                break;

            case (int)Reward.Skin:
                _music.Pause();
                break;
        }
    }

    private void OnRewardCallback(int id)
    {
        switch (_id)
        {
            case (int)Reward.Resources:
                _coins.Increase();
                _playerData.AddScore();
                Rewarded?.Invoke();
                break;

            case (int)Reward.Skin:
                _currentSkinView.OnBuySuccess();
                break;
        }

        _sfx.Play(_success);
        _saveLoader.Save();
    }

    private void OnCloseCallback()
    {
        switch (_id)
        {
            case (int)Reward.Resources:
                Time.timeScale = 1;
                break;

            case (int)Reward.Skin:
                _music.Continue();
                break;
        }
    }
}