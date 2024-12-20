using System;
using System.Collections;
using Game;
using LeaderboardBase;
using Money;
using PlayerBase;
using SaveSystem;

namespace CharacterBase
{
    public class Player : Character, IWinner
    {
        private PlayerDataSO _data;
        private SpawnChancesSO _spawnChancesSO;
        private SaveLoader _saveLoader;
        private Coins _coins;

        public event Action Victory;

        public void Init(PlayerDataSO data, SpawnChancesSO spawnChancesSO, SaveLoader saveLoader, Coins coins)
        {
            _data = data;
            _spawnChancesSO = spawnChancesSO;
            _saveLoader = saveLoader;
            _coins = coins;
        }

        protected override IEnumerator Win()
        {
            _coins.Increase();
            _data.AddScore();
            _spawnChancesSO.OnWin();
            _saveLoader.Save();
            Victory?.Invoke();
            StartCoroutine(base.Win());
            yield return null;
        }
    }
}