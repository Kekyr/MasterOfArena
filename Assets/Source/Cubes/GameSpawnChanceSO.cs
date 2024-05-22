using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new GameSpawnChanceSO", menuName = "GameSpawnChanceSO/Create new GameSpawnChanceSO")]
public class GameSpawnChanceSO : ScriptableObject
{
    [SerializeField] private List<SpawnChancesSO> _levelSpawnChances;

    public SpawnChancesSO GetLevelSpawnChance(int sceneBuildIndex)
    {
        int sceneIndex = sceneBuildIndex - 1;

        if (_levelSpawnChances.Count == 0)
        {
            throw new ArgumentOutOfRangeException(nameof(_levelSpawnChances));
        }

        return _levelSpawnChances[sceneIndex];
    }
}