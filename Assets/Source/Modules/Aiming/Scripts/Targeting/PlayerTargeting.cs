using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Aiming
{
    public class PlayerTargeting : Targeting
    {
        private readonly float _distanceFromCamera = 12;
        private readonly float _offsetZ = -5;

        private IInputRouter _inputRouter;
        private IManager _manager;
        private IMovable _projectile;

        private bool _canAim;

        protected override void OnEnable()
        {
            base.OnEnable();

            _projectile = _manager.Current;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _inputRouter.Aiming.performed -= OnFindingStarted;
            _inputRouter.Aiming.canceled -= OnFindingCanceled;
        }

        public void Init(IInputRouter playerInputRouter, IManager manager)
        {
            _manager = manager;
            _inputRouter = playerInputRouter;
            _inputRouter.Aiming.performed += OnFindingStarted;
            _inputRouter.Aiming.canceled += OnFindingCanceled;
        }

        private IEnumerator FindingTarget()
        {
            Vector3 throwDirection = Vector3.zero;

            while (_canAim)
            {
                throwDirection = TakeAim();
                RotateTo(throwDirection);
                yield return null;
            }

            InvokeAimed(throwDirection);
        }

        private Vector3 TakeAim()
        {
            Vector3 pointerScreenPosition = Pointer.current.position.ReadValue();

            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(
                new Vector3(pointerScreenPosition.x, pointerScreenPosition.y, _distanceFromCamera));

            mouseWorldPosition =
                new Vector3(mouseWorldPosition.x, transform.position.y, mouseWorldPosition.z + _offsetZ);

            Vector3 throwDirection = -(mouseWorldPosition - transform.position).normalized;

            return throwDirection;
        }

        private void OnFindingStarted(InputAction.CallbackContext context)
        {
            if (_projectile.IsMoving == false)
            {
                InvokeAiming();
                _canAim = true;
                StartCoroutine(FindingTarget());
            }
        }

        private void OnFindingCanceled(InputAction.CallbackContext context)
        {
            _canAim = false;
        }
    }
}