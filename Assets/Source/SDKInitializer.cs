using System;
using UnityEngine;
using YG;

public sealed class SDKInitializer : MonoBehaviour
{
    private SaveLoader _saveLoader;

    private void OnEnable()
    {
        YandexGame.GetDataEvent += OnInitialized;
    }

    private void OnDisable()
    {
        YandexGame.GetDataEvent -= OnInitialized;
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
        YandexGame.GameReadyAPI();
        _saveLoader.OnLoaded();
    }
}