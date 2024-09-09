using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
#if UNITY_WEBGL && !UNITY_EDITOR
using PlayerPrefs = Agava.YandexGames.Utility.PlayerPrefs;
#endif

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
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}