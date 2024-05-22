using System;
using DG.Tweening;
using UnityEngine;

public class Cube : MonoBehaviour
{
    private readonly float Duration = 0.1f;
    private readonly float Delay = 0.05f;

    [SerializeField] private uint _damage;
    [SerializeField] private CubeView _view;
    [SerializeField] private MeshRenderer _mesh;
    [SerializeField] private CubeExplosion _cubeExplosion;
    [SerializeField] private ColliderEventHandler _colliderEventHandler;

    private Collider _collider;
    private ParticleSystem _particleSystem;
    private Vector3 _startScale;

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

        if (_colliderEventHandler == null)
        {
            throw new ArgumentNullException(nameof(_colliderEventHandler));
        }

        _collider = _colliderEventHandler.GetComponent<Collider>();
        _particleSystem = _cubeExplosion.GetComponent<ParticleSystem>();

        _view.Init(_damage.ToString());

        _startScale = _mesh.transform.localScale;
        _mesh.transform.localScale = Vector3.zero;
        _mesh.transform.DOScale(_startScale, Duration)
            .SetEase(Ease.InOutSine)
            .SetDelay(Delay);

        _cubeExplosion.Stopped += OnCubeExplosionStopped;
        _colliderEventHandler.Collided += OnCollisionEnter;
    }

    private void OnDisable()
    {
        _cubeExplosion.Stopped -= OnCubeExplosionStopped;
        _colliderEventHandler.Collided -= OnCollisionEnter;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Projectile>(out Projectile projectile))
        {
            _mesh.transform.DOScale(0f, Duration)
                .SetEase(Ease.InOutSine)
                .OnComplete(() =>
                {
                    _particleSystem.Play();
                    _view.OnCollision(projectile.Character.DamageMarkColor);
                    projectile.Character.Attack(this);
                    _collider.enabled = false;
                });
        }
    }

    private void OnCubeExplosionStopped()
    {
        Collided?.Invoke(this);
    }
}