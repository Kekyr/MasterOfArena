using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Aiming
{
    public abstract class Targeting : MonoBehaviour
    {
        private readonly float _newScale = 0.02f;
        private readonly float _duration = 0.02f;

        [SerializeField] private Arrow _arrow;
        [SerializeField] private Circle _circle;

        private Sequence _sequence;
        private IReadOnlyCollection<IMortal> _mortals;

        public event Action<Vector3> Aimed;
        public event Action Aiming;

        protected virtual void OnEnable()
        {
            if (_arrow == null)
            {
                throw new ArgumentNullException(nameof(_arrow));
            }

            if (_circle == null)
            {
                throw new ArgumentNullException(nameof(_circle));
            }

            _circle.transform.localScale = Vector3.zero;

            _sequence.Append(_circle.transform.DOScale(_newScale, _duration)
                .SetEase(Ease.InBounce));
        }

        protected virtual void OnDisable()
        {
            foreach (IMortal mortal in _mortals)
            {
                mortal.Died -= OnDead;
            }
        }

        public void Init(Sequence sequence, IReadOnlyCollection<IMortal> mortals)
        {
            _mortals = mortals;
            _sequence = sequence;

            foreach (IMortal mortal in _mortals)
            {
                mortal.Died += OnDead;
            }

            enabled = true;
        }

        protected void RotateTo(Vector3 direction)
        {
            direction.y = 0;
            Quaternion newRotation = Quaternion.LookRotation(direction);
            transform.rotation = newRotation;
            _arrow.RotateTo(newRotation);
        }

        protected void InvokeAimed(Vector3 value)
        {
            Aimed?.Invoke(value);
        }

        protected void InvokeAiming()
        {
            Aiming?.Invoke();
        }

        private void OnDead()
        {
            enabled = false;
        }
    }
}