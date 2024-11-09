using System;
using PlayerBase;
using UnityEngine;

namespace Money
{
    public class Coins : MonoBehaviour
    {
        private PlayerDataSO _data;

        public event Action<int> Changed;

        private void OnEnable()
        {
            Changed?.Invoke(_data.Coins);
        }

        public void Init(PlayerDataSO data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            _data = data;
            enabled = true;
        }

        public void Increase()
        {
            _data.IncreaseCoins();
            Changed?.Invoke(_data.Coins);
        }

        public bool TryDecrease(int amount)
        {
            return _data.Coins - amount < 0;
        }

        public void Decrease(int amount)
        {
            _data.DecreaseCoins(amount);
            Changed?.Invoke(_data.Coins);
        }
    }
}