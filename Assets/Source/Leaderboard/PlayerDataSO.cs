using UnityEngine;

[CreateAssetMenu(fileName = "new PlayerDataSO", menuName = "PlayerDataSO/Create new PlayerDataSO")]
public class PlayerDataSO : ScriptableObject
{
    [SerializeField] private int _score;

    public int Score => _score;

    public void Init(int score)
    {
        _score = score;
    }
    
    public void AddScore()
    {
        _score++;
    }
}