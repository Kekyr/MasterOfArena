using UnityEngine;
using UnityEngine.SceneManagement;

public class InterstitialAd : MonoBehaviour
{
    public void Show()
    {
        Agava.YandexGames.InterstitialAd.Show(OnOpenCallback, OnCloseCallback);
    }

    private void OnOpenCallback()
    {
        Time.timeScale = 0;
    }

    private void OnCloseCallback(bool wasShown)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}