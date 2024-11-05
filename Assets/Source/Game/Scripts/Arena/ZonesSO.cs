using UnityEngine;

[CreateAssetMenu(fileName = "new ZonesSO", menuName = "ZonesSO/Create new ZonesSO")]
public class ZonesSO : ScriptableObject
{
    [SerializeField] private ZoneSO[] _zones;
    [SerializeField] private int _currentIndex;

    public ZoneSO Current => _zones[_currentIndex];

    public int CurrentIndex => _currentIndex;

    public ZoneSO Next
    {
        get
        {
            int index = _currentIndex + 1;

            if (index == _zones.Length)
            {
                index = 0;
            }

            return _zones[index];
        }
    }

    public void Init(int currentIndex)
    {
        _currentIndex = currentIndex;
    }

    public void Change()
    {
        int index = _currentIndex + 1;

        if (index == _zones.Length)
        {
            index = 0;
        }

        _currentIndex = index;
    }
}