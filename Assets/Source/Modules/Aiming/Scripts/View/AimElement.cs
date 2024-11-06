using DG.Tweening;
using UnityEngine;

namespace Aiming
{
    public class AimElement : MonoBehaviour
    {
        private readonly float _duration = 0.05f;
        private readonly float _minScale = 0f;

        [SerializeField] private float _maxScale;

        private IMortal _mortal;
        private IThrower _thrower;
        private Targeting _targeting;

        protected virtual void OnEnable()
        {
            transform.localScale = Vector3.zero;
            _thrower.Aimed += DecreaseScale;
            _targeting.Aiming += IncreaseScale;
        }

        protected virtual void OnDisable()
        {
            _thrower.Aimed -= DecreaseScale;
            _thrower.Won -= DecreaseScale;
            _targeting.Aiming -= IncreaseScale;
            _mortal.Died -= DecreaseScale;
        }

        public void Init(IThrower thrower, Targeting targeting, IMortal mortal)
        {
            _thrower = thrower;
            _targeting = targeting;
            _mortal = mortal;

            _mortal.Died += DecreaseScale;
            _thrower.Won += DecreaseScale;

            enabled = true;
        }

        protected void IncreaseScale()
        {
            ChangeScale(_maxScale);
        }

        private void DecreaseScale()
        {
            ChangeScale(_minScale);
        }

        private void ChangeScale(float endValue)
        {
            transform.DOScale(endValue, _duration)
                .SetEase(Ease.InOutSine);
        }
    }
}