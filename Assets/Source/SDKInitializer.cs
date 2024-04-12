using System.Collections;
using Agava.YandexGames;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class SDKInitializer : MonoBehaviour
{
    private int _nextSceneIndex;

    private void Awake()
    {
        _nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

#if UNITY_WEBGL && !UNITY_EDITOR
        YandexGamesSdk.CallbackLogging = true;
#endif
    }

    private IEnumerator Start()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        yield return YandexGamesSdk.Initialize(OnInitialized);
#else
        yield break;
#endif
    }

    private void OnInitialized()
    {
        YandexGamesSdk.GameReady();
        SceneManager.LoadScene(_nextSceneIndex);
    }
}