using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class LosePopup : Popup
    {
        private readonly float _newScale = 1f;

        [SerializeField] private Transform _upperPart;
        [SerializeField] private Transform _lowerPart;

        [SerializeField] private float Duration = 1f;

        protected override void OnEnable()
        {
            if (_upperPart == null)
            {
                throw new ArgumentNullException(nameof(_upperPart));
            }

            if (_lowerPart == null)
            {
                throw new ArgumentNullException(nameof(_lowerPart));
            }

            _upperPart.localScale = Vector3.zero;
            _lowerPart.localScale = Vector3.zero;

            _upperPart.DOScale(_newScale, Duration)
                .SetEase(Ease.OutBounce)
                .OnComplete(() =>
                {
                    _lowerPart.DOScale(_newScale, Duration)
                        .SetEase(Ease.InOutSine);
                });

            base.OnEnable();
        }

        protected override void Next()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}