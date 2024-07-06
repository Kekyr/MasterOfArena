using System;
using UnityEngine;

public class RewardedAd : MonoBehaviour
{
    private PlayerDataSO _playerData;

    public event Action<int> Rewarded;

    private void OnEnable()
    {
    }

    public void Init(PlayerDataSO playerData)
    {
        if (playerData == null)
        {
            throw new ArgumentNullException(nameof(playerData));
        }

        _playerData = playerData;
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
    }

    private void OnCloseCallback()
    {
        Time.timeScale = 1;
    }
}