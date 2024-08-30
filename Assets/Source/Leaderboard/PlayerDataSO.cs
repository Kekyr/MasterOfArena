using System;
using UnityEngine;

[CreateAssetMenu(fileName = "new PlayerDataSO", menuName = "PlayerDataSO/Create new PlayerDataSO")]
public class PlayerDataSO : ScriptableObject
{
    [SerializeField] private Player _skin;

    [SerializeField] private int _score;
    [SerializeField] private int _scoreReward;

    [SerializeField] private int _coins;
    [SerializeField] private int _coinsReward;

    public Player Skin => _skin;
    public int Score => _score;
    public int ScoreReward => _scoreReward;
    public int Coins => _coins;
    public int CoinsReward => _coinsReward;

    public void Init(int score, int coins, Player skin)
    {
        _score = score;
        _coins = coins;
        _skin = skin;
    }

    public void AddScore()
    {
        _score += _scoreReward;
    }

    public void AddCoins()
    {
        _coins += _coinsReward;
    }

    public void RemoveCoins(int amount)
    {
        if (amount < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount));
        }

        _coins -= amount;
    }

    public void ChangeSkin(Player skin)
    {
        _skin = skin;
    }
}