using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class ProjectileMovement : MonoBehaviour
{
    private readonly float RayDistance = 7;

    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Vector3 _halfExtents;

    [SerializeField] private float _maxDistance;
    [SerializeField] private float _flySpeed;
    [SerializeField] private float _returnSpeed;
    [SerializeField] private float _minRandomX;
    [SerializeField] private float _maxRandomX;

    private Projectile _projectile;
    private Aiming _aiming;

    private float _acceleration;
    private Vector3 _flyDirection;

    public event Action<Vector3> Ricocheted;

    public bool IsReturning { get; private set; }

    private void OnEnable()
    {
        if (_rigidbody == null)
            throw new ArgumentNullException(nameof(_rigidbody));

        _acceleration = _flySpeed;
        _aiming = _projectile.Character.GetComponent<Aiming>();

        _aiming.Aimed += OnAimed;
        _projectile.Ricocheting += OnRicocheting;
    }

    private void OnDisable()
    {
        _aiming.Aimed -= OnAimed;
        _projectile.Ricocheting -= OnRicocheting;
    }

    public void Init(Projectile projectile)
    {
        if (projectile == null)
            throw new ArgumentNullException(nameof(projectile));

        _projectile = projectile;
        enabled = true;
    }

    public IEnumerator Fly(Vector3 startPosition)
    {
        _rigidbody.isKinematic = false;

        while (_projectile.IsFlying)
        {
            float distance = Vector3.Distance(startPosition, transform.position);

            if (distance >= _maxDistance && IsReturning == false)
                Return();

            _rigidbody.velocity = _acceleration * _flyDirection;

            yield return null;
        }

        _acceleration = _flySpeed;
        IsReturning = false;
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.isKinematic = true;
    }

    public void Miss()
    {
        _acceleration = _returnSpeed;
        Return();
    }

    private Vector3 GenerateNewDirection()
    {
        if (IsReturning == false)
            Return();

        _flyDirection.x = Random.Range(_minRandomX, _maxRandomX);
        Rotate();

        return _flyDirection;
    }

    private void Return()
    {
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
        bool canRicochet = false;
        RaycastHit hit = default(RaycastHit);

        do
        {
            Vector3 direction = GenerateNewDirection();
            Physics.BoxCast(transform.position, _halfExtents, direction, out hit, transform.rotation, RayDistance);

            if (hit.point != Vector3.zero &&
                hit.collider.gameObject.TryGetComponent<CatchZone>(out CatchZone catchZone))
                canRicochet = true;
        } while (canRicochet == false);

        Ricocheted?.Invoke(hit.point);
    }
}