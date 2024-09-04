using System;
using UnityEngine;

public abstract class RewardedAd : MonoBehaviour
{
    public void Show()
    {
        Agava.YandexGames.VideoAd.Show(OnOpenCallback, OnRewardCallback, OnCloseCallback);
    }

    private void OnOpenCallback()
    {
        Time.timeScale = 0;
    }

    private void OnCloseCallback()
    {
        Time.timeScale = 1;
    }

    protected abstract void OnRewardCallback();
}