using System;
using UnityEngine;

public class Root : MonoBehaviour
{
    [SerializeField] private PlayerAiming _playerAiming;
    [SerializeField] private AiAiming _enemyAiming;

    [SerializeField] private CubeSpawner _cubeSpawner;

    [SerializeField] private Catcher _player;
    [SerializeField] private Catcher _enemy;

    [SerializeField] private Health _playerHealth;
    [SerializeField] private HealthView _playerHealthView;

    [SerializeField] private Health _enemyHealth;
    [SerializeField] private HealthView _enemyHealthView;

    [SerializeField] private Projectile[] _playerProjectiles;
    [SerializeField] private Projectile[] _enemyProjectiles;

    private PlayerInputRouter _inputRouter;

    private void Validate()
    {
        int maxProjectiles = 2;

        if (_playerAiming == null)
            throw new ArgumentNullException(nameof(_playerAiming));

        if (_enemyAiming == null)
            throw new ArgumentNullException(nameof(_enemyAiming));

        if (_player == null)
            throw new ArgumentNullException(nameof(_player));

        if (_enemy == null)
            throw new ArgumentNullException(nameof(_enemy));

        if (_cubeSpawner == null)
            throw new ArgumentNullException(nameof(_cubeSpawner));

        if (_playerHealth == null)
            throw new ArgumentNullException(nameof(_playerHealth));

        if (_playerHealthView == null)
            throw new ArgumentNullException(nameof(_playerHealthView));

        if (_enemyHealth == null)
            throw new ArgumentNullException(nameof(_enemyHealth));

        if (_enemyHealthView == null)
            throw new ArgumentNullException(nameof(_enemyHealthView));

        if (_playerProjectiles.Length == 0 || _playerProjectiles.Length > maxProjectiles)
            throw new ArgumentOutOfRangeException(nameof(_playerProjectiles));

        if (_enemyProjectiles.Length == 0 || _enemyProjectiles.Length > maxProjectiles)
            throw new ArgumentOutOfRangeException(nameof(_enemyProjectiles));

    }

    private void Awake()
    {
        Validate();

        _inputRouter = new PlayerInputRouter(_playerAiming);

        for (int i = 0; i < _playerProjectiles.Length; i++)
            _playerProjectiles[i].Init(_player);

        _player.Init(_playerProjectiles);

        for (int i = 0; i < _enemyProjectiles.Length; i++)
            _enemyProjectiles[i].Init(_enemy);

        _enemy.Init(_enemyProjectiles);

        _playerHealth.Init(_playerHealthView);

        _enemyHealth.Init(_enemyHealthView);

        _enemyAiming.Init(_cubeSpawner, _enemyProjectiles);
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