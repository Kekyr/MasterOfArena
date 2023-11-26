using UnityEngine;

[CreateAssetMenu(fileName = "new PoolSO", menuName = "PoolSO/Create new PoolSO")]
public class PoolSO : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private PooledObject _prefab;
    [SerializeField] private int _numberPerInstance;

    public string Name => _name;
    public PooledObject Prefab => _prefab;
    public int NumberPerInstance => _numberPerInstance;
}