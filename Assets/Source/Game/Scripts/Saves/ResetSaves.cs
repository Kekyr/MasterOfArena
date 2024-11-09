using System;
using UnityEngine;
using UnityEngine.UI;
using YG;

namespace SaveSystem
{
    public class ResetSaves : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private SaveLoader _saveLoader;

        private void OnEnable()
        {
            if (_button == null)
            {
                throw new ArgumentNullException(nameof(_button));
            }

            if (_saveLoader == null)
            {
                throw new ArgumentNullException(nameof(_saveLoader));
            }

            _button.onClick.AddListener(Reset);
        }

        private void Reset()
        {
            YandexGame.ResetSaveProgress();
            YandexGame.SaveProgress();
        }
    }
}