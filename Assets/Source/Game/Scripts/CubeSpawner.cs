using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField] private List<Transform> _spawnPositions = new List<Transform>();

    [SerializeField] private SpawnChancesSO _spawnChancesSO;

    [SerializeField] private uint _interval;

    private List<Cube> _cubes = new List<Cube>();

    private Health _playerHealth;
    private Health _enemyHealth;

    private WaitForSeconds _waitInterval;

    private void OnEnable()
    {
        if (_spawnPositions.Count == 0)
        {
            throw new ArgumentOutOfRangeException(nameof(_spawnPositions));
        }

        if (_spawnChancesSO == null)
        {
            throw new ArgumentNullException(nameof(_spawnChancesSO));
        }

        _waitInterval = new WaitForSeconds(_interval);

        foreach (Transform spawnPosition in _spawnPositions)
        {
            Spawn(spawnPosition);
        }
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
        _playerHealth = playerHealth;
        _enemyHealth = enemyHealth;

        _playerHealth.Died += OnDead;
        _enemyHealth.Died += OnDead;

        enabled = true;
    }

    public Vector3 GetRandomCubePosition()
    {
        int randomPositionIndex;

        do
        {
            randomPositionIndex = Random.Range(0, _spawnPositions.Count);
        }
        while (_spawnPositions[randomPositionIndex].childCount == 0);

        return _spawnPositions[randomPositionIndex].position;
    }

    private void Spawn(Transform spawnPosition)
    {
        float randomValue = Random.value * 100;
        Cube prefab = _spawnChancesSO.GetRandomCube(randomValue);
        Cube cube = Instantiate(
            prefab,
            spawnPosition.position,
            prefab.transform.rotation,
            spawnPosition);
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