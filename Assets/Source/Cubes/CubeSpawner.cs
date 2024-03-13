using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField] private List<Cube> _cubesPrefab = new List<Cube>();
    [SerializeField] private List<SpawnPosition> _spawnPositions = new List<SpawnPosition>();

    [SerializeField] private uint _interval;

    private List<Cube> _cubes = new List<Cube>();

    private Health _playerHealth;
    private Health _enemyHealth;
    private GameObject _explosion;

    private WaitForSeconds _waitInterval;

    private void OnEnable()
    {
        if (_cubesPrefab.Count == 0)
            throw new ArgumentOutOfRangeException(nameof(_cubesPrefab));

        if (_spawnPositions.Count == 0)
            throw new ArgumentOutOfRangeException(nameof(_spawnPositions));

        _waitInterval = new WaitForSeconds(_interval);

        foreach (SpawnPosition spawnPosition in _spawnPositions)
            Spawn(spawnPosition.transform);

        _playerHealth.Died += OnDead;
        _enemyHealth.Died += OnDead;
    }

    private void OnDisable()
    {
        foreach (Cube cube in _cubes)
            cube.Collided -= OnCollision;

        _playerHealth.Died -= OnDead;
        _enemyHealth.Died -= OnDead;
    }

    public void Init(Health playerHealth, Health enemyHealth, GameObject explosion)
    {
        if (playerHealth == null)
            throw new ArgumentNullException(nameof(playerHealth));

        if (enemyHealth == null)
            throw new ArgumentNullException(nameof(enemyHealth));

        if (explosion == null)
            throw new ArgumentNullException(nameof(explosion));

        _playerHealth = playerHealth;
        _enemyHealth = enemyHealth;
        _explosion = explosion;
        enabled = true;
    }

    public Vector3 GetRandomCubePosition()
    {
        int randomPositionIndex;

        do
        {
            randomPositionIndex = Random.Range(0, _spawnPositions.Count);
        } while (_spawnPositions[randomPositionIndex].transform.childCount == 0);

        return _spawnPositions[randomPositionIndex].transform.position;
    }

    private void Spawn(Transform spawnPosition)
    {
        int randomCubeIndex = Random.Range(0, _cubesPrefab.Count);
        Cube cube = Instantiate(_cubesPrefab[randomCubeIndex], spawnPosition.transform.position,
            _cubesPrefab[randomCubeIndex].transform.rotation,
            spawnPosition.transform);
        cube.Collided += OnCollision;
        cube.Init(_explosion);
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