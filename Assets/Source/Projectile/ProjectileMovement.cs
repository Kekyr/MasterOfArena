using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SFX))]
public class ProjectileMovement : MonoBehaviour
{
    private readonly float RayDistance = 7;
    private readonly float ReturnSpeed = 13;
    private readonly float FlySpeed = 6;

    [SerializeField] private float _minRandomX;
    [SerializeField] private float _maxRandomX;

    [SerializeField] private SFXSO _miss;
    [SerializeField] private Vector3 _halfExtents;

    private SFX _sfx;
    private Rigidbody _rigidbody;
    private Projectile _projectile;
    private Targeting _targeting;

    private float _acceleration;
    private Vector3 _flyDirection;

    public event Action<Vector3> Ricocheted;

    public bool IsReturning { get; private set; }

    private void OnEnable()
    {
        if (_miss == null)
        {
            throw new ArgumentNullException(nameof(_miss));
        }

        _acceleration = FlySpeed;
        _targeting = _projectile.Character.GetComponent<Targeting>();
        _rigidbody = GetComponent<Rigidbody>();
        _sfx = GetComponent<SFX>();

        _targeting.Aimed += OnAimed;
        _projectile.Ricocheting += OnRicocheting;
    }

    private void OnDisable()
    {
        _targeting.Aimed -= OnAimed;
        _projectile.Ricocheting -= OnRicocheting;
    }

    public void Init(Projectile projectile)
    {
        if (projectile == null)
        {
            throw new ArgumentNullException(nameof(projectile));
        }

        _projectile = projectile;
        enabled = true;
    }

    public IEnumerator Fly(Vector3 startPosition)
    {
        _rigidbody.isKinematic = false;

        while (_projectile.IsFlying)
        {
            float distance = Vector3.Distance(startPosition, transform.position);
            _rigidbody.velocity = _acceleration * _flyDirection;
            yield return null;
        }

        _acceleration = FlySpeed;
        IsReturning = false;
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.isKinematic = true;
    }

    public void Miss()
    {
        _sfx.Play(_miss);
        _acceleration = ReturnSpeed;
        Return();
    }

    private Vector3 GenerateNewDirection()
    {
        if (IsReturning == false)
        {
            Return();
        }

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
            {
                canRicochet = true;
            }
        } while (canRicochet == false);

        Ricocheted?.Invoke(hit.point);
    }
}