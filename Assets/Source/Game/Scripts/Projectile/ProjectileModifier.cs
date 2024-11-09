using CharacterBase;
using DG.Tweening;
using UnityEngine;

namespace ProjectileBase
{
    public class ProjectileModifier : MonoBehaviour
    {
        private readonly float _scaleModifier = 1.3f;
        private readonly float _scaleDuration = 0.05f;

        private Character _character;
        private Vector3 _originalScale;

        private void OnEnable()
        {
            _character.Throwed += OnThrow;
        }

        private void OnDisable()
        {
            _character.Throwed -= OnThrow;
        }

        public void Init(Character character)
        {
            _character = character;
            enabled = true;
        }

        public void ChangeScale()
        {
            Vector3 newScale = _originalScale * _scaleModifier;

            transform.DOScale(newScale, _scaleDuration)
                .SetEase(Ease.InOutSine)
                .OnComplete(() =>
                {
                    transform.DOScale(_originalScale, _scaleDuration)
                        .SetEase(Ease.InOutSine);
                });
        }

        private void OnThrow(Transform newParent)
        {
            _originalScale = transform.localScale;
        }
    }
}