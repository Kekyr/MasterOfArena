using System;
using DG.Tweening;
using UnityEngine;

public class CubeMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;

    [SerializeField] private float _yModifier;
    [SerializeField] private float _xModifier;
    [SerializeField] private float _duration;

    private void OnEnable()
    {
        if (_rigidbody == null)
        {
            throw new ArgumentNullException(nameof(_rigidbody));
        }

        Vector3 endPosition = new Vector3(transform.position.x + _xModifier, transform.position.y + _yModifier, transform.position.z);

        _rigidbody.DOMove(endPosition, _duration)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.Linear);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Projectile>(out Projectile projectile))
        {
            _rigidbody.DOKill();
        }
    }
}