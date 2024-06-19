using UnityEngine;

[CreateAssetMenu(fileName = "new ZoneSO", menuName = "ZoneSO/Create new ZoneSO")]
public class ZoneSO : ScriptableObject
{
    [SerializeField] private Zone _prefab;
    [SerializeField] private Sprite _sprite;

    public Sprite Icon => _sprite;
    public Zone Prefab => _prefab;
}