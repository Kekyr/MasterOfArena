using System;
using System.Collections;
using UnityEngine;

public class Player : Character
{
    private PlayerDataSO _data;
    private SpawnChancesSO _spawnChancesSO;
    private SaveLoader _saveLoader;

    public event Action<int> Victory;

    public void Init(PlayerDataSO data, SpawnChancesSO spawnChancesSO, SaveLoader saveLoader)
    {
        if (data == null)
        {
            throw new ArgumentNullException(nameof(data));
        }

        if (spawnChancesSO == null)
        {
            throw new ArgumentNullException(nameof(spawnChancesSO));
        }

        if (saveLoader == null)
        {
            throw new ArgumentNullException(nameof(saveLoader));
        }

        _data = data;
        _spawnChancesSO = spawnChancesSO;
        _saveLoader = saveLoader;
    }

    protected override IEnumerator Win()
    {
        _data.AddScore();
        _spawnChancesSO.OnWin();

#if UNITY_WEBGL && !UNITY_EDITOR
        _saveLoader.Save();
        Victory?.Invoke(_data.Score);
#endif

        StartCoroutine(base.Win());
        yield return null;
    }
}