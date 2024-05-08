using System;
using DG.Tweening;
using UnityEngine;

public class CubeMovement : MonoBehaviour
{
    private readonly float YModifier = 1.5f;

    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private float Duration;

    private void OnEnable()
    {
        if (_rigidbody == null)
        {
            throw new ArgumentNullException(nameof(_rigidbody));
        }

        _rigidbody.DOMoveY(transform.position.y + YModifier, Duration)
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