using System;
using UnityEngine;

public class InterstitialAd : MonoBehaviour
{
    public event Action Opened;
    public event Action Closed;

    public void Show()
    {
        Agava.YandexGames.InterstitialAd.Show(OnOpenCallback, OnCloseCallback);
    }

    private void OnOpenCallback()
    {
        Time.timeScale = 0;
        Opened?.Invoke();
    }

    private void OnCloseCallback(bool wasShown)
    {
        Time.timeScale = 1;
        Closed?.Invoke();
    }
}