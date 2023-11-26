using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private readonly float RayDistance = 7;

    [SerializeField] private ProjectileMovement _movement;
    [SerializeField] private Animator _animator;

    [SerializeField] private Transform _startFlyPosition;

    private Player _player;

    private Vector3 _startPosition;
    private Quaternion _startRotation;
    private Transform _startParent;

    public event Action<Vector3> Ricocheted;

    public bool IsFlying => transform.parent != _startParent;

    private void OnEnable()
    {
        if (_animator == null)
            throw new ArgumentNullException(nameof(_animator));

        if (_movement == null)
            throw new ArgumentNullException(nameof(_movement));

        if (_startFlyPosition == null)
            throw new ArgumentNullException(nameof(_startFlyPosition));

        Ricocheted += _player.OnRicochet;

        _movement.Init(this);
    }

    private void OnDisable()
    {
        Ricocheted -= _player.OnRicochet;
    }

    private void Start()
    {
        _startParent = transform.parent;
    }

    public void Init(Player player)
    {
        if (player == null)
            throw new ArgumentNullException(nameof(player));

        _player = player;
        enabled = true;
    }

    public void OnAimed(Vector3 flyDirection)
    {
        _startPosition = transform.localPosition;
        _startRotation = transform.localRotation;
        _movement.OnAimed(flyDirection);
    }

    public void OnThrow(Transform newParent)
    {
        transform.position = _startFlyPosition.transform.position;
        transform.eulerAngles = _startFlyPosition.transform.rotation.eulerAngles;
        transform.parent = newParent;

        _animator.enabled = true;

        StartCoroutine(_movement.Fly(_startPosition));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<Player>(out Player player) && IsFlying)
        {
            _animator.enabled = false;

            transform.parent = _startParent;
            transform.localPosition = _startPosition;
            transform.localRotation = _startRotation;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Cube>(out Cube cube) && _movement.IsReturning == false)
        {
            Vector3 direction = _movement.Ricochet();
            Physics.Raycast(transform.position, direction, out RaycastHit hit, RayDistance);
            Debug.DrawRay(transform.position, direction * RayDistance, Color.red, 3f);
            Ricocheted?.Invoke(hit.point);
        }
    }
}