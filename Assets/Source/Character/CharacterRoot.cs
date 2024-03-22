using System;
using DG.Tweening;
using UnityEngine;

public class CharacterRoot : MonoBehaviour
{
    [SerializeField] private Popup _popup;
    [SerializeField] private Projectile[] _projectiles;

    [SerializeField] private Character _character;
    [SerializeField] private Character _enemyCharacter;

    [SerializeField] private Movement _movement;

    [SerializeField] private Targeting _aiming;
    [SerializeField] private Arrow _arrow;
    [SerializeField] private Circle _circle;

    [SerializeField] private Health _health;
    [SerializeField] private HealthView _healthView;

    [SerializeField] private Health _enemyHealth;

    [SerializeField] private ArenaSide _side;
    [SerializeField] private BombPlatform _bombPlatform;

    private SFXButton _sfxButton;
    private AudioSettingsSO _audioSettings;
    private Sequence _sequence;

    protected virtual void OnEnable()
    {
        int maxProjectiles = 2;

        if (_popup == null)
            throw new ArgumentNullException(nameof(_popup));

        if (_projectiles.Length == 0 || _projectiles.Length > maxProjectiles)
            throw new ArgumentOutOfRangeException(nameof(_projectiles));

        if (_character == null)
            throw new ArgumentNullException(nameof(_character));

        if (_enemyCharacter == null)
            throw new ArgumentNullException(nameof(_enemyCharacter));

        if (_health == null)
            throw new ArgumentNullException(nameof(_health));

        if (_healthView == null)
            throw new ArgumentNullException(nameof(_healthView));

        if (_enemyHealth == null)
            throw new ArgumentNullException(nameof(_enemyHealth));

        if (_movement == null)
            throw new ArgumentNullException(nameof(_movement));

        if (_aiming == null)
            throw new ArgumentNullException(nameof(_aiming));

        if (_arrow == null)
            throw new ArgumentNullException(nameof(_arrow));

        if (_circle == null)
            throw new ArgumentNullException(nameof(_circle));

        if (_side == null)
            throw new ArgumentNullException(nameof(_side));

        if (_bombPlatform == null)
            throw new ArgumentNullException(nameof(_bombPlatform));
    }

    protected virtual void Start()
    {
        for (int i = 0; i < _projectiles.Length; i++)
        {
            _projectiles[i].Init(_character);
            _projectiles[i].GetComponent<SFX>().Init(_sfxButton, _audioSettings);
        }

        _character.Init(_projectiles, _sequence, _enemyHealth, _popup);
        _character.GetComponent<SFX>().Init(_sfxButton, _audioSettings);

        _movement.Init(_projectiles, _health);

        _side.Init(_health);
        _side.GetComponent<SFX>().Init(_sfxButton, _audioSettings);

        _healthView.Init(_health);

        _health.Init(_bombPlatform);

        _bombPlatform.Init(_sequence, _health, _enemyCharacter);

        _aiming.Init(_sequence, _health, _enemyHealth);
        _arrow.Init(_character, _aiming, _health);
        
        _circle.Init(_projectiles);
        _circle.Init(_character, _aiming, _health);
    }

    public void Init(Sequence sequence, SFXButton sfxButton, AudioSettingsSO audioSettings)
    {
        _sequence = sequence;
        _sfxButton = sfxButton;
        _audioSettings = audioSettings;
        enabled = true;
    }
}