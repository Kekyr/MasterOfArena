using System;
using DG.Tweening;
using UnityEngine;

namespace CubeFeature
{
    [RequireComponent(typeof(Rigidbody))]
    public class Cube : MonoBehaviour
    {
        private readonly float _duration = 0.1f;
        private readonly float _delay = 0.05f;

        [SerializeField] private CubeView _view;
        [SerializeField] private MeshRenderer _mesh;
        [SerializeField] private CubeExplosion _cubeExplosion;
        [SerializeField] private Collider _collider;

        [SerializeField] private uint _damage;

        private Rigidbody _rigidbody;
        private ParticleSystem _particleSystem;
        private Vector3 _startScale;
        private bool _collided;

        public event Action<Cube> Collided;

        public uint Damage => _damage;

        private void OnEnable()
        {
            if (_view == null)
            {
                throw new ArgumentNullException(nameof(_view));
            }

            if (_mesh == null)
            {
                throw new ArgumentNullException(nameof(_mesh));
            }

            if (_cubeExplosion == null)
            {
                throw new ArgumentNullException(nameof(_cubeExplosion));
            }

            if (_collider == null)
            {
                throw new ArgumentNullException(nameof(_collider));
            }

            _rigidbody = GetComponent<Rigidbody>();
            _particleSystem = _cubeExplosion.GetComponent<ParticleSystem>();

            _view.Init(_damage.ToString());

            _startScale = _mesh.transform.localScale;
            _mesh.transform.localScale = Vector3.zero;
            _mesh.transform.DOScale(_startScale, _duration)
                .SetEase(Ease.InOutSine)
                .SetDelay(_delay);

            _cubeExplosion.Stopped += OnCubeExplosionStopped;
        }

        private void OnDisable()
        {
            _cubeExplosion.Stopped -= OnCubeExplosionStopped;
        }

        public void OnCollision(IAttacker attacker)
        {
            if (_collided == false)
            {
                _collided = true;
                _rigidbody.DOKill();

                _mesh.transform.DOScale(0f, _duration)
                    .SetEase(Ease.InOutSine)
                    .OnComplete(() =>
                    {
                        _particleSystem.Play();
                        _view.OnCollision(attacker.DamageMarkColor);
                        attacker.Attack(this);
                        _collider.enabled = false;
                    });
            }
        }

        private void OnCubeExplosionStopped()
        {
            Collided?.Invoke(this);
        }
    }
}