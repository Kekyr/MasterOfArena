using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_WEBGL && !UNITY_EDITOR
using PlayerPrefs = Agava.YandexGames.Utility.PlayerPrefs;
#endif

public class SaveLoader : MonoBehaviour
{
    private readonly string key = "Save";

    private ProgressBarSO _progressBarData;
    private PlayerDataSO _playerData;
    private ZonesSO _zonesData;
    private SpawnChancesSO _spawnChancesData;
    private SkinDataSO[] _skinsData;
    private TutorialSO _tutorialData;
    private AudioSettingsSO _audioSettings;

    private MusicButton _musicButton;
    private SFXButton _sfxButton;
    private ImageSO _musicButtonData;
    private ImageSO _sfxButtonData;

    private void OnDisable()
    {
        _musicButton.Switched -= Save;
        _sfxButton.Switched -= Save;
    }

    public void Init(ProgressBarSO progressBarData, PlayerDataSO playerData, ZonesSO zonesData,
        SpawnChancesSO spawnChancesData, SkinDataSO[] skinsData, TutorialSO tutorialData, AudioSettingsSO audioSettings,
        ImageSO musicButtonData, ImageSO sfxButtonData)
    {
        _progressBarData = progressBarData;
        _playerData = playerData;
        _zonesData = zonesData;
        _spawnChancesData = spawnChancesData;
        _skinsData = skinsData;
        _tutorialData = tutorialData;
        _audioSettings = audioSettings;
        _musicButtonData = musicButtonData;
        _sfxButtonData = sfxButtonData;
    }

    public void Init(MusicButton musicButton, SFXButton sfxButton)
    {
        _musicButton = musicButton;
        _sfxButton = sfxButton;
        _musicButton.Switched += Save;
        _sfxButton.Switched += Save;
    }

    public void Save()
    {
        List<State> skinsState = new List<State>();
        int[] cubesIndex =
        {
            _spawnChancesData.FirstElementIndex,
            _spawnChancesData.SecondElementIndex,
            _spawnChancesData.ThirdElementIndex
        };

        foreach (SkinDataSO skinData in _skinsData)
        {
            skinsState.Add(skinData.Status);
        }

        SaveData saveData = new SaveData(_progressBarData.CurrentPointIndex, _progressBarData.StartSliderValue,
            _progressBarData.EndSliderValue, _playerData.Score, _playerData.Coins, _zonesData.CurrentIndex,
            _spawnChancesData.SpawnChances, cubesIndex,
            skinsState, _playerData.CurrentSkinIndex, _tutorialData.CanPlay, _audioSettings.IsMusicOn,
            _musicButtonData.CurrentIndex,
            _audioSettings.IsSFXOn, _sfxButtonData.CurrentIndex);

        string json = JsonUtility.ToJson(saveData);

        PlayerPrefs.SetString(key, json);
        PlayerPrefs.Save();
    }

    public void OnLoaded()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        int firstElementIndex = 0;
        SaveData saveData;

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

        for (int i = 0; i < saveData.CubesIndex.Length; i++)
        {
            Debug.Log(saveData.CubesIndex[i]);
        }

        _progressBarData.Init(saveData.CurrentPointIndex, saveData.StartBarValue, saveData.EndBarValue);
        _playerData.Init(saveData.Score, saveData.Coins, saveData.CurrentSkinIndex);
        _zonesData.Init(saveData.CurrentZoneIndex);
        _spawnChancesData.Init(saveData.SpawnChances, saveData.CubesIndex);
        _tutorialData.Init(saveData.CanPlay);
        _audioSettings.Init(saveData.IsMusicOn, saveData.IsSFXOn);
        _musicButtonData.Init(saveData.MusicSpriteIndex);
        _sfxButtonData.Init(saveData.SFXSpriteIndex);

        SceneManager.LoadScene(nextSceneIndex);
    }
}