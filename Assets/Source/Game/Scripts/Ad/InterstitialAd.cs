using UnityEngine;
using UnityEngine.SceneManagement;
using YG;

public class InterstitialAd : MonoBehaviour
{
    private void OnEnable()
    {
        YandexGame.OpenFullAdEvent += OnOpenCallback;
        YandexGame.CloseFullAdEvent += OnCloseCallback;
    }

    private void OnDisable()
    {
        YandexGame.OpenFullAdEvent -= OnOpenCallback;
        YandexGame.CloseFullAdEvent -= OnCloseCallback;
    }

    public void Show()
    {
        YandexGame.FullscreenShow();
    }

    private void OnOpenCallback()
    {
        Time.timeScale = 0;
    }

    private void OnCloseCallback()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}