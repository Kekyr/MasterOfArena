using System;
using UnityEngine;

public class Root : MonoBehaviour
{
    [SerializeField] private Aiming _aiming;
    [SerializeField] private Player _player;
    [SerializeField] private Projectile _projectile;

    private PlayerInputRouter _inputRouter;

    private void Awake()
    {
        if (_aiming == null)
            throw new ArgumentNullException(nameof(_aiming));

        if (_player == null)
            throw new ArgumentNullException(nameof(_player));

        _inputRouter = new PlayerInputRouter(_aiming);

        _projectile.Init();

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