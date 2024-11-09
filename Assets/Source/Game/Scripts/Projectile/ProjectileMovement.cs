using System;
using Aiming;
using Audio;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ProjectileBase
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(SFX))]
    public class ProjectileMovement : MonoBehaviour, IMovable
    {
        private readonly float _returnSpeed = 13;
        private readonly float _flySpeed = 6;

        [SerializeField] private SFXSO _miss;
        [SerializeField] private Collider _catch;

        private SFX _sfx;
        private Rigidbody _rigidbody;
        private Projectile _projectile;
        private Targeting _targeting;

        private Collider _catchZone;

        private float _minRandomX;
        private float _maxRandomX;

        private bool _isMoving;
        private float _acceleration;
        private Vector3 _flyDirection;

        public event Action<Vector3> Ricocheted;

        public bool IsReturning { get; private set; }

        public bool IsMoving => _isMoving;

        private void OnEnable()
        {
            if (_miss == null)
            {
                throw new ArgumentNullException(nameof(_miss));
            }

            if (_catch == null)
            {
                throw new ArgumentNullException(nameof(_catch));
            }

            _acceleration = _flySpeed;
            _targeting = _projectile.Character.GetComponent<Targeting>();
            _rigidbody = GetComponent<Rigidbody>();
            _sfx = GetComponent<SFX>();

            _minRandomX = _catchZone.bounds.min.x;
            _maxRandomX = _catchZone.bounds.max.x;

            _targeting.Aimed += OnAimed;
            _projectile.Ricocheting += OnRicocheting;
        }

        private void OnDisable()
        {
            _targeting.Aimed -= OnAimed;
            _projectile.Ricocheting -= OnRicocheting;
        }

        private void FixedUpdate()
        {
            if (_isMoving == true)
            {
                _rigidbody.velocity = _acceleration * _flyDirection;
            }
        }

        public void Init(Projectile projectile)
        {
            _projectile = projectile;
            enabled = true;
        }

        public void Init(Collider catchZone)
        {
            _catchZone = catchZone;
        }

        public void StartFly(Vector3 startPosition)
        {
            _rigidbody.isKinematic = false;
            _isMoving = true;
        }

        public void StopFly()
        {
            _isMoving = false;
            _acceleration = _flySpeed;
            IsReturning = false;
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.isKinematic = true;
        }

        public void Miss()
        {
            _sfx.Play(_miss);
            _acceleration = _returnSpeed;
            Return();
        }

        private Vector3 GenerateNewDirection()
        {
            float endPointY = 0.5f;

            float randomX = Random.Range(_minRandomX, _maxRandomX);
            Vector3 endPoint = new Vector3(randomX, endPointY, _catchZone.transform.position.z);
            _flyDirection = (endPoint - transform.position).normalized;
            Rotate();

            _catch.enabled = true;
            IsReturning = true;

            return endPoint;
        }

        private void Return()
        {
            _catch.enabled = true;
            _flyDirection = -_flyDirection;
            IsReturning = true;
        }

        private void Rotate()
        {
            Quaternion newRotation = Quaternion.LookRotation(_flyDirection);
            Vector3 eulerAngles = newRotation.eulerAngles;
            eulerAngles.z = transform.rotation.eulerAngles.z;
            transform.eulerAngles = eulerAngles;
        }

        private void OnAimed(Vector3 flyDirection)
        {
            flyDirection.y = 0;
            _flyDirection = flyDirection;
        }

        private void OnRicocheting()
        {
            Vector3 endPoint = GenerateNewDirection();
            Ricocheted?.Invoke(endPoint);
        }
    }
}