using System;
using UnityEngine;

public class AIRoot : CharacterRoot
{
    [SerializeField] private AiTargeting _targeting;
    [SerializeField] private Projectile[] _playerProjectiles;

    private CubeSpawner _cubeSpawner;

    protected override void OnEnable()
    {
        base.OnEnable();

        int maxProjectiles = 2;

        if (_targeting == null)
            throw new ArgumentNullException(nameof(_targeting));

        if (_playerProjectiles.Length == 0 || _playerProjectiles.Length > maxProjectiles)
            throw new ArgumentOutOfRangeException(nameof(_playerProjectiles));
    }

    protected override void Start()
    {
        _targeting.Init(_cubeSpawner, _playerProjectiles);
        base.Start();
    }

    public void Init(CubeSpawner cubeSpawner)
    {
        _cubeSpawner = cubeSpawner;
    }
}