using UnityEngine;

[CreateAssetMenu(fileName = "new ImageRankSO", menuName = "ImageRankSO/Create new ImageRankSO")]
public class ImageRankSO : ScriptableObject
{
    [SerializeField] private Sprite[] _sprites;

    public Sprite[] Sprites => _sprites;
}