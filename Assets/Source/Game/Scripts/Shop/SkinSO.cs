using CharacterBase;
using UnityEngine;

namespace ShopSystem
{
    [CreateAssetMenu(fileName = "new SkinSO", menuName = "SkinSO/Create new SkinSO")]
    public class SkinSO : ScriptableObject
    {
        [SerializeField] private SkinDataSO _data;
        [SerializeField] private Player _prefab;

        public SkinDataSO Data => _data;
        public Player Prefab => _prefab;
    }
}