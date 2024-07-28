using System;
using System.Collections;
using UnityEngine;

public class Player : Character
{
    [SerializeField] private PlayerDataSO _data;
    [SerializeField] private SpawnChancesSO _spawnChancesSO;

    private SaveLoader _saveLoader;

    public event Action<int> Victory;

    protected override void OnEnable()
    {
        base.OnEnable();

        if (_data == null)
        {
            throw new ArgumentNullException(nameof(_data));
        }

        if (_spawnChancesSO == null)
        {
            throw new ArgumentNullException(nameof(_spawnChancesSO));
        }
    }

    public void Init(SaveLoader saveLoader)
    {
        if (saveLoader == null)
        {
            throw new ArgumentNullException(nameof(saveLoader));
        }

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