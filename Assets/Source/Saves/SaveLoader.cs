using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using PlayerPrefs = Agava.YandexGames.Utility.PlayerPrefs;

public class SaveLoader : MonoBehaviour
{
    private readonly string key = "Save";

    [SerializeField] private ProgressBarSO _progressBarData;
    [SerializeField] private PlayerDataSO _playerData;
    [SerializeField] private ZonesSO _zonesData;
    [SerializeField] private SpawnChancesSO _spawnChancesData;
    [SerializeField] private TutorialSO _tutorialData;

    private void OnEnable()
    {
        if (_progressBarData == null)
        {
            throw new ArgumentNullException(nameof(_progressBarData));
        }

        if (_playerData == null)
        {
            throw new ArgumentNullException(nameof(_playerData));
        }

        if (_zonesData == null)
        {
            throw new ArgumentNullException(nameof(_zonesData));
        }

        if (_spawnChancesData == null)
        {
            throw new ArgumentNullException(nameof(_spawnChancesData));
        }

        if (_tutorialData == null)
        {
            throw new ArgumentNullException(nameof(_tutorialData));
        }
    }

    public void Save()
    {
        SaveData saveData = new SaveData(_progressBarData.CurrentPointIndex, _progressBarData.StartSliderValue,
            _progressBarData.EndSliderValue,
            _playerData.Score, _zonesData.CurrentIndex, _spawnChancesData.SpawnChances, _tutorialData.CanPlay);

        string json = JsonUtility.ToJson(saveData);

        PlayerPrefs.SetString(key, json);
        PlayerPrefs.Save();
    }

    public void OnLoaded()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        SaveData saveData = JsonUtility.FromJson<SaveData>(PlayerPrefs.GetString(key));

        if (saveData == null)
        {
            return;
        }

        _progressBarData.Init(saveData.CurrentPointIndex, saveData.StartBarValue, saveData.EndBarValue);
        _playerData.Init(saveData.Score);
        _zonesData.Init(saveData.CurrentZoneIndex);
        _spawnChancesData.Init(saveData.SpawnChances);
        _tutorialData.Init(saveData.CanPlay);

        SceneManager.LoadScene(nextSceneIndex);
    }
}