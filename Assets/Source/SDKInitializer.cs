using System;
using System.Collections;
using Agava.YandexGames;
using UnityEngine;
using UnityEngine.SceneManagement;
using PlayerPrefs = Agava.YandexGames.Utility.PlayerPrefs;

public sealed class SDKInitializer : MonoBehaviour
{
    private SaveLoader _saveLoader;

    private void Awake()
    {
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

    public void Init(SaveLoader saveLoader)
    {
        if (saveLoader == null)
        {
            throw new ArgumentNullException(nameof(saveLoader));
        }

        _saveLoader = saveLoader;
        enabled = true;
    }

    private void OnInitialized()
    {
        YandexGamesSdk.GameReady();
        PlayerPrefs.Load(_saveLoader.OnLoaded);
    }
}