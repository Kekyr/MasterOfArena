using CharacterBase;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "new EnemiesSO", menuName = "EnemiesSO/Create new EnemiesSO")]
    public class EnemiesSO : ScriptableObject
    {
        [SerializeField] private Character[] _prefabs;

        public Character GetRandom()
        {
            int randomIndex = Random.Range(0, _prefabs.Length);
            return _prefabs[randomIndex];
        }
    }
}