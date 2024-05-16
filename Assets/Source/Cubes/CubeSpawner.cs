using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField] private List<SpawnPosition> _spawnPositions = new List<SpawnPosition>();

    [SerializeField] private GameSpawnChanceSO _gameSpawnChanceSO;

    [SerializeField] private uint _interval;

    private List<Cube> _cubes = new List<Cube>();

    private LevelSpawnChanceSO _levelSpawnChanceSO;
    private Health _playerHealth;
    private Health _enemyHealth;

    private WaitForSeconds _waitInterval;

    private void OnEnable()
    {
        if (_spawnPositions.Count == 0)
        {
            throw new ArgumentOutOfRangeException(nameof(_spawnPositions));
        }

        if (_gameSpawnChanceSO == null)
        {
            throw new ArgumentNullException(nameof(_gameSpawnChanceSO));
        }

        _levelSpawnChanceSO = _gameSpawnChanceSO.GetLevelSpawnChance(SceneManager.GetActiveScene().buildIndex);
        _waitInterval = new WaitForSeconds(_interval);

        foreach (SpawnPosition spawnPosition in _spawnPositions)
        {
            Spawn(spawnPosition.transform);
        }

        _playerHealth.Died += OnDead;
        _enemyHealth.Died += OnDead;
    }

    private void OnDisable()
    {
        foreach (Cube cube in _cubes)
        {
            cube.Collided -= OnCollision;
        }

        _playerHealth.Died -= OnDead;
        _enemyHealth.Died -= OnDead;
    }

    public void Init(Health playerHealth, Health enemyHealth)
    {
        if (playerHealth == null)
        {
            throw new ArgumentNullException(nameof(playerHealth));
        }

        if (enemyHealth == null)
        {
            throw new ArgumentNullException(nameof(enemyHealth));
        }

        _playerHealth = playerHealth;
        _enemyHealth = enemyHealth;
        enabled = true;
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

    private void Spawn(Transform spawnPosition)
    {
        float randomValue = Random.value * 100;
        Debug.Log($"Random.value:{randomValue}");
        Cube prefab = _levelSpawnChanceSO.GetRandomCube(randomValue);
        Cube cube = Instantiate(prefab, spawnPosition.transform.position, prefab.transform.rotation, spawnPosition.transform);
        cube.Collided += OnCollision;
        _cubes.Add(cube);
    }

    private IEnumerator Replace(Cube cube)
    {
        yield return _waitInterval;
        _cubes.Remove(cube);
        Transform spawnPosition = cube.transform.parent;
        Destroy(cube.gameObject);
        Spawn(spawnPosition);
    }

    private void OnCollision(Cube cube)
    {
        StartCoroutine(Replace(cube));
    }

    private void OnDead()
    {
        gameObject.SetActive(false);
    }
}