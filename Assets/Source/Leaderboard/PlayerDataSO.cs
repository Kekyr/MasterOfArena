using UnityEngine;

[CreateAssetMenu(fileName = "new PlayerDataSO", menuName = "PlayerDataSO/Create new PlayerDataSO")]
public class PlayerDataSO : ScriptableObject
{
    [SerializeField] private Player _currentSkin;

    [SerializeField] private int _score;
    [SerializeField] private int _scoreReward;

    [SerializeField] private int _coins;
    [SerializeField] private int _coinsReward;

    public int Score => _score;
    public int ScoreReward => _scoreReward;

    public int Coins => _coins;
    public int CoinsReward => _coinsReward;

    public void Init(int score, int coins)
    {
        _score = score;
        _coins = coins;
    }

    public void AddScore()
    {
        _score += _scoreReward;
    }

    public void AddCoins()
    {
        _coins += _coinsReward;
    }
}