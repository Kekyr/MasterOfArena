using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new SkinsSO", menuName = "SkinsSO/Create new SkinsSO")]
public class SkinsSO : ScriptableObject
{
    [SerializeField] private List<SkinSO> _skins;

    public List<SkinSO> Skins => _skins;
}