using UnityEngine;

public class Zone : MonoBehaviour
{
    [SerializeField] private ArenaSide _playerSide;
    [SerializeField] private ArenaSide _enemySide;

    public ArenaSide PlayerSide => _playerSide;
    public ArenaSide EnemySide => _enemySide;
}