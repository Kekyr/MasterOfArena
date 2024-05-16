using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new LevelSpawnChanceSO", menuName = "LevelSpawnChanceSO/Create new LevelSpawnChanceSO")]
public class LevelSpawnChanceSO : ScriptableObject
{
    [SerializeField] private List<float> _spawnChances;
    [SerializeField] private List<Cube> _cubePrefabs;

    public Cube GetRandomCube(float randomValue)
    {
        if (_cubePrefabs.Count != _spawnChances.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(_spawnChances.Count));
        }

        float minValue = 0;
        float maxValue = _spawnChances[0];

        for (int i = 0; i < _cubePrefabs.Count; i++)
        {
            Debug.Log($"minValue={minValue} maxValue={maxValue}");
            if (randomValue >= minValue && randomValue <= maxValue)
            {
                return _cubePrefabs[i];
            }

            minValue += _spawnChances[i];
            maxValue += _spawnChances[i + 1];
        }

        return null;
    }
}