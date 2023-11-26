using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    private Dictionary<string, Queue<PooledObject>> _poolDictionary = new Dictionary<string, Queue<PooledObject>>();

    public void AddPool(PoolSO pool)
    {
        Queue<PooledObject> objectPool;
        GameObject poolParent;

        if (_poolDictionary.ContainsKey(pool.Name) == false)
        {
            poolParent = new GameObject(pool.Name);
            poolParent.transform.parent = transform;

            objectPool = new Queue<PooledObject>();
            _poolDictionary.Add(pool.Name, objectPool);

            CreateInstances(pool, objectPool, poolParent);
        }
    }

    private void CreateInstances(PoolSO pool, Queue<PooledObject> objectPool, GameObject poolParent)
    {
        for (int i = 0; i < pool.NumberPerInstance; i++)
        {
            PooledObject projectile = Instantiate(pool.Prefab, poolParent.transform);
            projectile.gameObject.SetActive(false);
            objectPool.Enqueue(projectile);
        }
    }

    public PooledObject SpawnFromPool(string poolName, Vector3 position, Quaternion rotation)
    {
        if (_poolDictionary.ContainsKey(poolName) == false)
            throw new ArgumentOutOfRangeException(nameof(poolName));

        PooledObject objectToSpawn = _poolDictionary[poolName].Dequeue();

        objectToSpawn.gameObject.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        _poolDictionary[poolName].Enqueue(objectToSpawn);

        return objectToSpawn;
    }
}