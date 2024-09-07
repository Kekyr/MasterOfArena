using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using PlayerPrefs = Agava.YandexGames.Utility.PlayerPrefs;

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
#if UNITY_WEBGL && !UNITY_EDITOR
PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("Deleted all keys");
#else
        _saveLoader.OnLoaded();
#endif
    }
}