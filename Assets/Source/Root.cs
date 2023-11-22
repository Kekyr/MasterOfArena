using System;
using UnityEngine;

public class Root : MonoBehaviour
{
    [SerializeField] private Aiming _aiming;
    [SerializeField] private Player _player;
    [SerializeField] private Projectile _projectile;

    private PlayerInputRouter _inputRouter;

    private void Validate()
    {
        if (_aiming == null)
            throw new ArgumentNullException(nameof(_aiming));

        if (_player == null)
            throw new ArgumentNullException(nameof(_player));

        if (_projectile == null)
            throw new ArgumentNullException(nameof(_projectile));
    }

    private void Awake()
    {
        Validate();

        _inputRouter = new PlayerInputRouter(_aiming);

        _projectile.Init(_player);

        _player.Init(_projectile);

        _aiming.Init(_player);
    }

    private void OnEnable()
    {
        _inputRouter.OnEnable();
    }

    private void OnDisable()
    {
        _inputRouter.OnDisable();
    }
}