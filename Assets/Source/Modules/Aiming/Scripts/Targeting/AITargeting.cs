using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aiming
{
    public class AITargeting : Targeting
    {
        private readonly float _delay = 2f;

        private IReadOnlyCollection<ICatchable> _projectiles;
        private ISpawner _spawner;
        private WaitForSeconds _waitForSeconds;

        protected override void OnEnable()
        {
            base.OnEnable();

            foreach (ICatchable projectile in _projectiles)
            {
                projectile.Catched += OnCatch;
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            foreach (ICatchable projectile in _projectiles)
            {
                projectile.Catched -= OnCatch;
            }
        }

        private void Start()
        {
            _waitForSeconds = new WaitForSeconds(_delay);
            OnCatch();
        }

        public void Init(ISpawner spawner, IReadOnlyCollection<ICatchable> projectiles)
        {
            _spawner = spawner;
            _projectiles = projectiles;
        }

        private IEnumerator FindingTarget()
        {
            InvokeAiming();
            yield return _waitForSeconds;

            Vector3 throwDirection = (_spawner.GetRandomPosition() - transform.position).normalized;
            RotateTo(throwDirection);
            InvokeAimed(throwDirection);
        }

        private void OnCatch()
        {
            StartCoroutine(FindingTarget());
        }
    }
}