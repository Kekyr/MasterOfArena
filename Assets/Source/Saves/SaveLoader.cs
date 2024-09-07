using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using PlayerPrefs = Agava.YandexGames.Utility.PlayerPrefs;

public class SaveLoader : MonoBehaviour
{
    private readonly string key = "Save";

    private ProgressBarSO _progressBarData;
    private PlayerDataSO _playerData;
    private ZonesSO _zonesData;
    private SpawnChancesSO _spawnChancesData;
    private SkinDataSO[] _skinsData;
    private TutorialSO _tutorialData;

    public void Init(ProgressBarSO progressBarData, PlayerDataSO playerData, ZonesSO zonesData,
        SpawnChancesSO spawnChancesData, SkinDataSO[] skinsData, TutorialSO tutorialData)
    {
        _progressBarData = progressBarData;
        _playerData = playerData;
        _zonesData = zonesData;
        _spawnChancesData = spawnChancesData;
        _skinsData = skinsData;
        _tutorialData = tutorialData;
    }

    public void Save()
    {
        List<State> skinsState = new List<State>();

        foreach (SkinDataSO skinData in _skinsData)
        {
            skinsState.Add(skinData.Status);
        }

        SaveData saveData = new SaveData(_progressBarData.CurrentPointIndex, _progressBarData.StartSliderValue,
            _progressBarData.EndSliderValue, _playerData.Score, _playerData.Coins, _zonesData.CurrentIndex,
            _spawnChancesData.SpawnChances,
            skinsState, _playerData.CurrentSkinIndex, _tutorialData.CanPlay);

        string json = JsonUtility.ToJson(saveData);

        PlayerPrefs.SetString(key, json);
        PlayerPrefs.Save();
    }

    public void OnLoaded()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
#else
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex;
#endif
        
        int firstElementIndex = 0;
        SaveData saveData;

#if UNITY_WEBGL && !UNITY_EDITOR
        if (PlayerPrefs.HasKey(key))
        {
            Debug.Log("Using old savedata");
            saveData = JsonUtility.FromJson<SaveData>(PlayerPrefs.GetString(key));
        }
        else
        {
            Debug.Log("Creating new savedata");
            saveData = new SaveData();
        }
#else
        saveData = new SaveData();
#endif

        if (saveData.SkinsState == null)
        {
            for (int i = 0; i < _skinsData.Length; i++)
            {
                if (i == firstElementIndex)
                {
                    _skinsData[i].Init(State.Selected);
                    continue;
                }

                _skinsData[i].Init(State.NotBought);
            }
        }
        else
        {
            for (int i = 0; i < _skinsData.Length; i++)
            {
                _skinsData[i].Init(saveData.SkinsState[i]);
            }
        }

        _progressBarData.Init(saveData.CurrentPointIndex, saveData.StartBarValue, saveData.EndBarValue);
        _playerData.Init(saveData.Score, saveData.Coins, saveData.CurrentSkinIndex);
        _zonesData.Init(saveData.CurrentZoneIndex);
        _spawnChancesData.Init(saveData.SpawnChances);
        _tutorialData.Init(saveData.CanPlay);

        SceneManager.LoadScene(nextSceneIndex);
    }
}