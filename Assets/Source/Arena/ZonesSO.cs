using UnityEngine;

[CreateAssetMenu(fileName = "new ZonesSO", menuName = "ZonesSO/Create new ZonesSO")]
public class ZonesSO : ScriptableObject
{
    [SerializeField] private Zone[] _zonePrefabs;
    [SerializeField] private int _currentIndex;

    public Zone Current
    {
        get
        {
            return _zonePrefabs[_currentIndex];
        }
    }
}