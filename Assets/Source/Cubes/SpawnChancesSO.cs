using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new SpawnChanceSO", menuName = "SpawnChanceSO/Create new SpawnChanceSO")]
public class SpawnChancesSO : ScriptableObject
{
    [SerializeField] private List<float> _spawnChances;
    [SerializeField] private List<Cube> _cubePrefabs;

    private int _firstElementIndex = 0;
    private int _secondElementIndex = 1;
    private int _thirdElementIndex;

    public List<float> SpawnChances => _spawnChances;

    public void Init(List<float> spawnChances)
    {
        _spawnChances = spawnChances;
        _firstElementIndex = 0;
        _secondElementIndex = 1;
        _thirdElementIndex = 0;
    }

    public Cube GetRandomCube(float randomValue)
    {
        if (_cubePrefabs.Count < _spawnChances.Count)
        {
            int index = 1;
            int count = _spawnChances.Count - 2;
            int startChance = 50;
            int firstElementIndex = 0;
            int secondElementIndex = 1;

            _spawnChances.RemoveRange(index, count);

            _spawnChances[firstElementIndex] = startChance;
            _spawnChances[secondElementIndex] = startChance;
        }

        float minValue = 0;
        float maxValue = _spawnChances[0];

        for (int i = 0; i < _spawnChances.Count; i++)
        {
            if (randomValue >= minValue && randomValue <= maxValue)
            {
                return _cubePrefabs[i];
            }

            minValue += _spawnChances[i];
            maxValue += _spawnChances[i + 1];
        }

        return null;
    }

    public void OnWin()
    {
        int _endChance = 50;
        int _firstStep = 10;
        int _secondStep = 5;

        if (_spawnChances[_firstElementIndex] == _endChance && _spawnChances[_secondElementIndex] == _endChance)
        {
            _spawnChances.Add(0);
            _thirdElementIndex = _spawnChances.Count - 1;
        }

        if (_spawnChances[_secondElementIndex] != _spawnChances[_thirdElementIndex])
        {
            if (_spawnChances[_secondElementIndex] - _firstStep < _spawnChances[_thirdElementIndex] + _firstStep)
            {
                Trade(_secondElementIndex, _thirdElementIndex, _secondStep);
                return;
            }

            Trade(_secondElementIndex, _thirdElementIndex, _firstStep);
            return;
        }

        if (_spawnChances[_firstElementIndex] != 0)
        {
            Trade(_firstElementIndex, _secondElementIndex, _secondStep);
            Trade(_firstElementIndex, _thirdElementIndex, _secondStep);
            return;
        }

        _firstElementIndex = _secondElementIndex;
        _secondElementIndex = _thirdElementIndex;
    }

    private void Trade(int firstElementIndex, int secondElementIndex, int step)
    {
        _spawnChances[firstElementIndex] -= step;
        _spawnChances[secondElementIndex] += step;
    }
}