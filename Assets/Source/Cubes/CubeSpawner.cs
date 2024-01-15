using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField] private List<Cube> _cubesPrefab = new List<Cube>();
    [SerializeField] private List<SpawnPosition> _spawnPositions = new List<SpawnPosition>();

    [SerializeField] private uint _delay;

    private List<Cube> _cubes = new List<Cube>();

    private Health _playerHealth;
    private Health _enemyHealth;
    private WaitForSeconds _wait;

    private void OnEnable()
    {
        if (_cubesPrefab.Count == 0)
            throw new ArgumentOutOfRangeException(nameof(_cubesPrefab));

        if (_spawnPositions.Count == 0)
            throw new ArgumentOutOfRangeException(nameof(_spawnPositions));

        _wait = new WaitForSeconds(_delay);

        foreach (SpawnPosition spawnPosition in _spawnPositions)
            Spawn(spawnPosition);

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

    public void Init(Health playerHealth, Health enemyHealth)
    {
        if (playerHealth == null)
            throw new ArgumentNullException(nameof(playerHealth));

        if (enemyHealth == null)
            throw new ArgumentNullException(nameof(enemyHealth));

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

    private void Spawn(SpawnPosition spawnPosition)
    {
        int randomCubeIndex = Random.Range(0, _cubesPrefab.Count);
        Cube cube = Instantiate(_cubesPrefab[randomCubeIndex], spawnPosition.transform.position, Quaternion.identity,
            spawnPosition.transform);
        cube.Collided += OnCollision;
        _cubes.Add(cube);
    }

    private IEnumerator Replace(Cube cube)
    {
        _cubes.Remove(cube);
        SpawnPosition spawnPosition = cube.transform.parent.gameObject.GetComponent<SpawnPosition>();
        Destroy(cube.gameObject);
        yield return _wait;
        Spawn(spawnPosition);
    }

    private void OnCollision(Collision collision, Cube cube)
    {
        if (collision.gameObject.TryGetComponent(out Projectile projectile) == false)
            return;

        if (projectile.Catcher.gameObject.TryGetComponent(out AiAiming aiAiming))
        {
            _playerHealth.ApplyDamage(cube.Damage);
        }
        else
        {
            _enemyHealth.ApplyDamage(cube.Damage);
        }

        StartCoroutine(Replace(cube));
    }

    private void OnDead()
    {
        gameObject.SetActive(false);
    }
}