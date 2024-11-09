using UnityEngine;

namespace Arena
{
    [CreateAssetMenu(fileName = "new ZoneSO", menuName = "ZoneSO/Create new ZoneSO")]
    public class ZoneSO : ScriptableObject
    {
        [SerializeField] private Zone _prefab;
        [SerializeField] private Sprite _sprite;
        [SerializeField] private string _translationName;

        public Sprite Icon => _sprite;
        public Zone Prefab => _prefab;
        public string TranslationName => _translationName;
    }
}