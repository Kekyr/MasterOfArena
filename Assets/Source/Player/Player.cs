using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    private readonly int Throw = Animator.StringToHash("Throw");

    [SerializeField] private Animator _animator;
    [SerializeField] private PlayerMovement _movement;

    private Projectile _projectile;

    private void OnEnable()
    {
        if (_animator == null)
            throw new ArgumentNullException(nameof(_animator));

        if (_movement == null)
            throw new ArgumentNullException(nameof(_movement));

        _movement.Init();
    }

    public void Init(Projectile projectile)
    {
        if (projectile == null)
            throw new ArgumentNullException(nameof(projectile));

        _projectile = projectile;
        enabled = true;
    }

    public void OnAimed(Vector3 flyDirection)
    {
        _projectile.OnAimed(flyDirection);
        _animator.SetTrigger(Throw);
    }

    public void OnRicochet(Vector3 endPoint)
    {
        endPoint = new Vector3(endPoint.x, transform.position.y, transform.position.z);
        _movement.OnRicochet(endPoint);
    }

    private void OnThrow()
    {
        _projectile.OnThrow(transform.parent);
    }
}