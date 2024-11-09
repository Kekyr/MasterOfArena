using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Money
{
    public class CoinsView : MonoBehaviour
    {
        private readonly float _duration = 1f;

        [SerializeField] private TextMeshProUGUI _textMeshPro;

        private float _startScale;
        private Coins _coins;

        private void OnEnable()
        {
            if (_textMeshPro == null)
            {
                throw new ArgumentNullException(nameof(_textMeshPro));
            }

            _startScale = transform.localScale.x;
            transform.localScale = Vector3.zero;
            transform.DOScale(_startScale, _duration)
                .SetEase(Ease.InOutSine);

            _coins.Changed += OnCoinsChanged;
        }

        private void OnDisable()
        {
            _coins.Changed -= OnCoinsChanged;
        }

        public void Init(Coins coins)
        {
            if (coins == null)
            {
                throw new ArgumentNullException(nameof(coins));
            }

            _coins = coins;
            enabled = true;
        }

        private void OnCoinsChanged(int coins)
        {
            _textMeshPro.text = coins.ToString();
        }
    }
}