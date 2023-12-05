using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField] private List<Cube> _cubes = new List<Cube>();
    [SerializeField] private List<SpawnPosition> _spawnPositions = new List<SpawnPosition>();

    private void OnEnable()
    {
        if (_cubes.Count == 0)
            throw new ArgumentOutOfRangeException(nameof(_cubes));

        if (_spawnPositions.Count == 0)
            throw new ArgumentOutOfRangeException(nameof(_spawnPositions));

        for (int i = 0; i < _spawnPositions.Count; i++)
        {
            int randomCubeIndex = Random.Range(0, _cubes.Count);
            Cube cube = Instantiate(_cubes[randomCubeIndex], _spawnPositions[i].transform.position, Quaternion.identity, _spawnPositions[i].transform);
            cube.Init();
        }
    }

    public Vector3 GetRandomCubePosition()
    {
        int randomPositionIndex;

        do
        {
            randomPositionIndex = Random.Range(0, _spawnPositions.Count);
        }
        while (_spawnPositions[randomPositionIndex].transform.childCount == 0);

        return _spawnPositions[randomPositionIndex].transform.position;
    }
}