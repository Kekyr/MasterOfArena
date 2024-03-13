using System;
using DG.Tweening;
using UnityEngine;

public class Cube : MonoBehaviour
{
    private readonly float Duration = 0.05f;
    private readonly float Delay = 0.05f;
    private readonly string VFX = "ExplosionVFX";

    [SerializeField] private uint _damage;
    [SerializeField] private CubeView _view;
    [SerializeField] private MeshRenderer _mesh;
    [SerializeField] private Collider _collider;

    private GameObject _explosion;
    private VFX _explosionVFX;
    private ParticleSystem _explosionParticleSystem;
    private Vector3 _startScale;
    public event Action<Cube> Collided;

    public uint Damage => _damage;

    private void OnEnable()
    {
        if (_view == null)
            throw new ArgumentNullException(nameof(_view));

        if (_mesh == null)
            throw new ArgumentNullException(nameof(_mesh));

        if (_collider == null)
            throw new ArgumentNullException(nameof(_collider));

        _view.Init(_damage.ToString());

        _startScale = _mesh.transform.localScale;
        _mesh.transform.localScale = Vector3.zero;
        _mesh.transform.DOScale(_startScale, Duration)
            .SetEase(Ease.InOutSine)
            .SetDelay(Delay);
    }

    private void OnDisable()
    {
        _explosionVFX.Stopped -= OnParticleSystemStopped;
    }

    private void Start()
    {
        GameObject vfx = Instantiate(_explosion, transform.position, transform.rotation, transform);
        _explosionParticleSystem = vfx.GetComponent<ParticleSystem>();
        _explosionVFX = vfx.GetComponent<VFX>();
        _explosionVFX.Stopped += OnParticleSystemStopped;
    }

    public void Init(GameObject explosion)
    {
        if (explosion == null)
            throw new ArgumentNullException(nameof(explosion));

        Debug.Log($"Cube: {explosion == null}");

        _explosion = explosion;
        enabled = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Projectile>(out Projectile projectile))
        {
            _mesh.transform.DOScale(0f, Duration)
                .SetEase(Ease.InOutSine)
                .OnComplete(() =>
                {
                    _explosionParticleSystem.Play();
                    _view.OnCollision(projectile.Character.DamageMarkColor);
                    projectile.Character.Attack(this);
                    _collider.enabled = false;
                });
        }
    }

    private void OnParticleSystemStopped()
    {
        Collided?.Invoke(this);
    }
}