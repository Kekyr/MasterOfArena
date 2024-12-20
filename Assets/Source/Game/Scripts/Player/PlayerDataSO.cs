using System;
using LeaderboardBase;
using ProgressBarFeature;
using UnityEngine;

namespace PlayerBase
{
    [CreateAssetMenu(fileName = "new PlayerDataSO", menuName = "PlayerDataSO/Create new PlayerDataSO")]
    public class PlayerDataSO : ScriptableObject, ILeaderboardData, IRewardsData
    {
        [SerializeField] private int _currentSkinIndex;

        [SerializeField] private int _score;
        [SerializeField] private int _scoreReward;

        [SerializeField] private int _coins;
        [SerializeField] private int _coinsReward;

        public int CurrentSkinIndex => _currentSkinIndex;
        public int Score => _score;
        public int ScoreReward => _scoreReward;
        public int Coins => _coins;
        public int CoinsReward => _coinsReward;

        public void Init(int score, int coins, int skinIndex)
        {
            _score = score;
            _coins = coins;
            _currentSkinIndex = skinIndex;
        }

        public void AddScore()
        {
            _score += _scoreReward;
        }

        public void IncreaseCoins()
        {
            _coins += _coinsReward;
        }

        public void DecreaseCoins(int amount)
        {
            if (amount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amount));
            }

            _coins -= amount;
        }

        public void ChangeSkin(int skinIndex)
        {
            _currentSkinIndex = skinIndex;
        }
    }
}