using System;
using DG.Tweening;
using UnityEngine;

public class Cube : MonoBehaviour
{
    private readonly float NewScale = 1f;
    private readonly float Duration = 0.05f;
    private readonly float Delay = 0.05f;

    [SerializeField] private uint _damage;
    [SerializeField] private CubeView _view;
    [SerializeField] private ParticleSystem _explosionVFX;
    [SerializeField] private MeshRenderer _mesh;
    [SerializeField] private Collider _collider;

    public event Action<Cube> Collided;

    public uint Damage => _damage;

    private void OnEnable()
    {
        if (_view == null)
            throw new ArgumentNullException(nameof(_view));

        if (_explosionVFX == null)
            throw new ArgumentNullException(nameof(_explosionVFX));

        if (_mesh == null)
            throw new ArgumentNullException(nameof(_mesh));

        if (_collider == null)
            throw new ArgumentNullException(nameof(_collider));

        _view.Init(_damage.ToString());

        _mesh.transform.localScale = Vector3.zero;
        _mesh.transform.DOScale(NewScale, Duration)
            .SetEase(Ease.InOutSine)
            .SetDelay(Delay);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Projectile>(out Projectile projectile))
        {
            _mesh.transform.DOScale(0f, Duration)
                .SetEase(Ease.InOutSine)
                .OnComplete(() =>
                {
                    _explosionVFX.Play();
                    _view.OnCollision(projectile.Catcher.DamageMarkColor);
                    projectile.Catcher.Attack(this);
                    _collider.enabled = false;
                });
        }
    }

    private void OnParticleSystemStopped()
    {
        Collided?.Invoke(this);
    }
}