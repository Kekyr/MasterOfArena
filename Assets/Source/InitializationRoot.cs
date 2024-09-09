using System;
using UnityEngine;

public class InitializationRoot : MonoBehaviour
{
    [SerializeField] private ProgressBarSO _progressBarData;
    [SerializeField] private PlayerDataSO _playerData;
    [SerializeField] private ZonesSO _zonesData;
    [SerializeField] private SpawnChancesSO _spawnChancesData;
    [SerializeField] private TutorialSO _tutorialData;
    [SerializeField] private SkinsSO _skins;
    [SerializeField] private AudioSettingsSO _audioSettings;
    [SerializeField] private ImageSO _musicButtonData;
    [SerializeField] private ImageSO _sfxButtonData;

    [SerializeField] private SDKInitializer _sdkInitializer;
    [SerializeField] private SaveLoader _saveLoader;

    private void Validate()
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

        if (_skins == null)
        {
            throw new ArgumentNullException(nameof(_skins));
        }

        if (_audioSettings == null)
        {
            throw new ArgumentNullException(nameof(_audioSettings));
        }

        if (_musicButtonData == null)
        {
            throw new ArgumentNullException(nameof(_musicButtonData));
        }

        if (_sfxButtonData == null)
        {
            throw new ArgumentNullException(nameof(_sfxButtonData));
        }

        if (_sdkInitializer == null)
        {
            throw new ArgumentNullException(nameof(_sdkInitializer));
        }

        if (_saveLoader == null)
        {
            throw new ArgumentNullException(nameof(_saveLoader));
        }
    }

    private void Awake()
    {
        Validate();

        SkinDataSO[] skinsData = new SkinDataSO[_skins.Skins.Count];

        for (int i = 0; i < _skins.Skins.Count; i++)
        {
            skinsData[i] = _skins.Skins[i].Data;
        }

        _saveLoader.Init(_progressBarData, _playerData, _zonesData, _spawnChancesData, skinsData, _tutorialData,
            _audioSettings, _musicButtonData, _sfxButtonData);
        _sdkInitializer.Init(_saveLoader);
    }
}