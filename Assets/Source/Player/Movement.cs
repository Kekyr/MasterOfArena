using System;
using System.Collections;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private readonly int IsRunning = Animator.StringToHash("IsRunning");

    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Animator _animator;

    [SerializeField] private float _acceleration;

    private Quaternion _startRotation;
    private float _minDistance = 0.1f;

    private void OnEnable()
    {
        if (_rigidbody == null)
            throw new ArgumentNullException(nameof(_rigidbody));

        if (_animator == null)
            throw new ArgumentNullException(nameof(_animator));
    }

    private void Awake()
    {
        _startRotation = transform.rotation;
    }

    public void OnRicochet(Vector3 endPosition)
    {
        StartCoroutine(Move(endPosition));
    }

    private IEnumerator Move(Vector3 endPosition)
    {
        float distance = 1;
        Vector3 direction = (endPosition - transform.position).normalized;

        transform.rotation = Quaternion.LookRotation(direction);
        _animator.SetBool(IsRunning, true);

        while (distance > _minDistance)
        {
            _rigidbody.velocity = _acceleration * direction;
            distance = Vector3.Distance(endPosition, transform.position);
            yield return null;
        }

        _animator.SetBool(IsRunning, false);
        _rigidbody.velocity = Vector3.zero;
        transform.rotation = _startRotation;
    }
}