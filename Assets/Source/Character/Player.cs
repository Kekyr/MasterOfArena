using System;
using System.Collections;
using UnityEngine;

public class Player : Character
{
    private PlayerDataSO _data;
    private SpawnChancesSO _spawnChancesSO;
    private SaveLoader _saveLoader;
    private Coins _coins;

    public event Action<int> Victory;

    public void Init(PlayerDataSO data, SpawnChancesSO spawnChancesSO, SaveLoader saveLoader, Coins coins)
    {
        _data = data;
        _spawnChancesSO = spawnChancesSO;
        _saveLoader = saveLoader;
        _coins = coins;
    }

    protected override IEnumerator Win()
    {
        _coins.Increase();
        _data.AddScore();
        _spawnChancesSO.OnWin();
        _saveLoader.Save();

#if UNITY_WEBGL && !UNITY_EDITOR
        Victory?.Invoke(_data.Score);
#endif

        StartCoroutine(base.Win());
        yield return null;
    }
}