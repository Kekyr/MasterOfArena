using UnityEngine;

[CreateAssetMenu(fileName = "new CharactersSO", menuName = "CharactersSO/Create new CharactersSO")]
public class CharactersSO : ScriptableObject
{
    [SerializeField] private Player[] _playerPrefabs;
    [SerializeField] private Character[] _enemyPrefabs;

    public Character GetRandomEnemyPrefab()
    {
        int randomIndex = Random.Range(0, _enemyPrefabs.Length);
        return _enemyPrefabs[randomIndex];
    }

    public Character GetRandomPlayerPrefab()
    {
        int randomIndex = Random.Range(0, _playerPrefabs.Length);
        return _playerPrefabs[randomIndex];
    }
}