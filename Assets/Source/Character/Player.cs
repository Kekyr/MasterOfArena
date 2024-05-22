using System;
using System.Collections;
using UnityEngine;

public class Player : Character
{
    [SerializeField] private PlayerDataSO _data;
    [SerializeField] private SpawnChancesSO _spawnChancesSO;

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

    protected override IEnumerator Win()
    {
        _data.AddScore();
        _spawnChancesSO.OnWin();

#if UNITY_WEBGL && !UNITY_EDITOR
        Victory?.Invoke(_data.Score);
#endif

        StartCoroutine(base.Win());
        yield return null;
    }
}