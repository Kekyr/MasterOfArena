using System;
using Aiming;
using BombPlatformFeature;
using UnityEngine;

namespace HealthSystem
{
    public class Health : MonoBehaviour, IValueGiver, IMortal, Audio.IMortal
    {
        [SerializeField] private uint _start;

        private float _current;

        private BombPlatform _platform;

        public event Action<float> ValueChanged;

        public event Action Died;

        public bool IsDead => _current <= 0;

        private void OnEnable()
        {
            _current = _start;
            ValueChanged?.Invoke(_current);

            _platform.Attacked += OnAttacked;
        }

        private void OnDisable()
        {
            _platform.Attacked -= OnAttacked;
        }

        public void Init(BombPlatform platform)
        {
            _platform = platform;
            enabled = true;
        }

        private void OnAttacked(uint damage)
        {
            _current -= damage;

            ValueChanged?.Invoke(_current);

            if (IsDead)
            {
                Died?.Invoke();
            }
        }
    }
}