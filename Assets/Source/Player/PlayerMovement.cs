using System;
using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private readonly int IsRunning = Animator.StringToHash("IsRunning");

    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Animator _animator;

    [SerializeField] private float _acceleration;

    private float _minDistance = 0.02f;

    private void OnEnable()
    {
        if (_rigidbody == null)
            throw new ArgumentNullException(nameof(_rigidbody));

        if (_animator == null)
            throw new ArgumentNullException(nameof(_animator));
    }

    public void Init()
    {
        enabled = true;
    }

    public void OnRicochet(Vector3 endPosition)
    {
        StartCoroutine(Move(endPosition));
    }

    private IEnumerator Move(Vector3 endPosition)
    {
        Vector3 direction = (endPosition - transform.position).normalized;
        float distance = 1;

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
        transform.rotation = Quaternion.LookRotation(Vector3.forward);
    }
}