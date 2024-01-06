using System;
using DG.Tweening;
using UnityEngine;

public class Root : MonoBehaviour
{
    [SerializeField] private Movement _playerMovement;
    [SerializeField] private Movement _enemyMovement;

    [SerializeField] private PlayerAiming _playerAiming;
    [SerializeField] private AiAiming _enemyAiming;

    [SerializeField] private ArenaSide _playerSide;
    [SerializeField] private ArenaSide _enemySide;

    [SerializeField] private BombPlatform _playerPlatform;
    [SerializeField] private BombPlatform _enemyPlatform;

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

        if (_playerMovement == null)
            throw new ArgumentNullException(nameof(_playerMovement));

        if (_enemyMovement == null)
            throw new ArgumentNullException(nameof(_enemyMovement));

        if (_playerAiming == null)
            throw new ArgumentNullException(nameof(_playerAiming));

        if (_enemyAiming == null)
            throw new ArgumentNullException(nameof(_enemyAiming));

        if (_playerSide == null)
            throw new ArgumentNullException(nameof(_playerSide));

        if (_enemySide == null)
            throw new ArgumentNullException(nameof(_enemySide));

        if (_playerPlatform == null)
            throw new ArgumentNullException(nameof(_playerPlatform));

        if (_enemyPlatform == null)
            throw new ArgumentNullException(nameof(_enemyPlatform));

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

        Sequence sequence = DOTween.Sequence();

        _inputRouter = new PlayerInputRouter();

        for (int i = 0; i < _playerProjectiles.Length; i++)
            _playerProjectiles[i].Init(_player);

        _player.Init(_playerProjectiles, sequence);
        _playerMovement.Init(_playerProjectiles);

        for (int i = 0; i < _enemyProjectiles.Length; i++)
            _enemyProjectiles[i].Init(_enemy);

        _enemy.Init(_enemyProjectiles, sequence);
        _enemyMovement.Init(_enemyProjectiles);

        _playerSide.Init(_playerHealth);
        _enemySide.Init(_enemyHealth);

        _playerHealthView.Init(_playerHealth);
        _enemyHealthView.Init(_enemyHealth);

        _cubeSpawner.Init(_playerHealth, _enemyHealth);

        _playerHealth.Init();
        _enemyHealth.Init();

        _playerPlatform.Init(sequence, _playerHealth);
        _enemyPlatform.Init(sequence, _enemyHealth);

        _playerAiming.Init(sequence);
        _playerAiming.Init(_inputRouter.Aiming);

        _enemyAiming.Init(sequence);
        _enemyAiming.Init(_cubeSpawner);
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