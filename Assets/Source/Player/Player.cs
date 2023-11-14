using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public readonly int Throw = Animator.StringToHash("Throw");

    [SerializeField] private Animator _animator;

    private Projectile _projectile;

    private void OnEnable()
    {
        if (_animator == null)
            throw new ArgumentNullException(nameof(_animator));
    }

    public void Init(Projectile projectile)
    {
        if (projectile == null)
            throw new ArgumentNullException(nameof(projectile));

        _projectile = projectile;
        enabled = true;
    }

    public void OnAimed()
    {
        _animator.SetTrigger(Throw);
    }

    public void OnThrow()
    {
        _projectile.OnThrow(transform.parent);
    }
}