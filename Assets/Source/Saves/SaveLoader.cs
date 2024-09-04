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
    private SkinSO[] _skinsData;
    private TutorialSO _tutorialData;

    public void Init(ProgressBarSO progressBarData, PlayerDataSO playerData, ZonesSO zonesData,
        SpawnChancesSO spawnChancesData, SkinSO[] skinsData, TutorialSO tutorialData)
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

        foreach (SkinSO skinData in _skinsData)
        {
            skinsState.Add(skinData.Status);
        }

        SaveData saveData = new SaveData(_progressBarData.CurrentPointIndex, _progressBarData.StartSliderValue,
            _progressBarData.EndSliderValue, _playerData.Score, _playerData.Coins, _zonesData.CurrentIndex,
            _spawnChancesData.SpawnChances,
            skinsState, _playerData.Skin, _tutorialData.CanPlay);

        string json = JsonUtility.ToJson(saveData);

        PlayerPrefs.SetString(key, json);
        PlayerPrefs.Save();
    }

    public void OnLoaded()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        int firstElementIndex = 0;
        Player currentSkin;

        SaveData saveData = JsonUtility.FromJson<SaveData>(PlayerPrefs.GetString(key));

        if (saveData == null)
        {
            saveData = new SaveData();
        }

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

        if (saveData.CurrentSkin == null)
        {
            currentSkin = _skinsData[firstElementIndex].Prefab;
        }
        else
        {
            currentSkin = saveData.CurrentSkin;
        }

        _progressBarData.Init(saveData.CurrentPointIndex, saveData.StartBarValue, saveData.EndBarValue);
        _playerData.Init(saveData.Score, saveData.Coins, currentSkin);
        _zonesData.Init(saveData.CurrentZoneIndex);
        _spawnChancesData.Init(saveData.SpawnChances);
        _tutorialData.Init(saveData.CanPlay);

        SceneManager.LoadScene(nextSceneIndex);
    }
}