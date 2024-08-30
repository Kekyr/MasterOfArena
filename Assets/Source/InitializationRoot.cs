using System;
using UnityEngine;

public class InitializationRoot : MonoBehaviour
{
    [SerializeField] private ProgressBarSO _progressBarData;
    [SerializeField] private PlayerDataSO _playerData;
    [SerializeField] private ZonesSO _zonesData;
    [SerializeField] private SpawnChancesSO _spawnChancesData;
    [SerializeField] private TutorialSO _tutorialData;
    [SerializeField] private SkinSO[] _skinsData;

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

        if (_skinsData.Length == 0)
        {
            throw new ArgumentNullException(nameof(_skinsData));
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

        _saveLoader.Init(_progressBarData, _playerData, _zonesData, _spawnChancesData, _skinsData, _tutorialData);
        _sdkInitializer.Init(_saveLoader);
    }
}