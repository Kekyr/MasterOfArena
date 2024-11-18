using System;
using SaveSystem;
using UnityEngine;
using YG;

namespace Game
{
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
            _saveLoader = saveLoader;
            enabled = true;
        }

        private void OnInitialized()
        {
            YandexGame.GameReadyAPI();
            _saveLoader.OnLoaded();
        }
    }
}