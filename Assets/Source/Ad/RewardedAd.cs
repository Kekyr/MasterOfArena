using System;
using UnityEngine;

public class RewardedAd : MonoBehaviour
{
    private PlayerDataSO _playerData;
    private SaveLoader _saveLoader;

    public event Action<int> Rewarded;

    public void Init(PlayerDataSO playerData, SaveLoader saveLoader)
    {
        if (playerData == null)
        {
            throw new ArgumentNullException(nameof(playerData));
        }

        if (saveLoader == null)
        {
            throw new ArgumentNullException(nameof(saveLoader));
        }

        _playerData = playerData;
        _saveLoader = saveLoader;
        enabled = true;
    }

    public void Show()
    {
        Agava.YandexGames.VideoAd.Show(OnOpenCallback, OnRewardCallback, OnCloseCallback);
    }

    private void OnOpenCallback()
    {
        Time.timeScale = 0;
    }

    private void OnRewardCallback()
    {
        _playerData.AddScore();
        Rewarded?.Invoke(_playerData.Score);
        _saveLoader.Save();
    }

    private void OnCloseCallback()
    {
        Time.timeScale = 1;
    }
}