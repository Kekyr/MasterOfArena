using System;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using UnityEngine;

public class CharacterRoot : MonoBehaviour
{
    [SerializeField] private Helper _helper;
    [SerializeField] private Popup _popup;

    [SerializeField] private HealthView _healthView;

    [SerializeField] private ArenaSide _side;
    [SerializeField] private BombPlatform _bombPlatform;

    private Character _character;
    private Character _enemyCharacter;

    private Health _health;
    private Health _enemyHealth;

    private Movement _movement;
    private Targeting _aiming;

    private Projectile[] _projectiles;

    private Arrow _arrow;
    private Circle _circle;

    private SFXButton _sfxButton;
    private AudioSettingsSO _audioSettings;

    private Sequence _sequence;
    private CinemachineVirtualCamera _camera;

    public IReadOnlyCollection<Projectile> Projectiles => _projectiles;

    public Targeting Aiming => _aiming;

    public Character Person => _character;

    private void OnEnable()
    {
        if (_popup == null)
        {
            throw new ArgumentNullException(nameof(_popup));
        }

        if (_healthView == null)
        {
            throw new ArgumentNullException(nameof(_healthView));
        }

        if (_side == null)
        {
            throw new ArgumentNullException(nameof(_side));
        }

        if (_bombPlatform == null)
        {
            throw new ArgumentNullException(nameof(_bombPlatform));
        }

        if (_helper == null)
        {
            throw new ArgumentNullException(nameof(_helper));
        }

        _projectiles = _character.GetComponentsInChildren<Projectile>();
        _movement = _character.GetComponent<Movement>();
        _aiming = _character.GetComponent<Targeting>();
        _arrow = _character.GetComponentInChildren<Arrow>();
        _circle = _character.GetComponentInChildren<Circle>();
        _health = _character.GetComponent<Health>();
        _enemyHealth = _enemyCharacter.GetComponent<Health>();
    }

    protected virtual void Start()
    {
        for (int i = 0; i < _projectiles.Length; i++)
        {
            _projectiles[i].Init(_character, _helper);
            _projectiles[i].GetComponent<SFX>().Init(_sfxButton, _audioSettings);
        }

        _character.Init(_projectiles, _sequence, _enemyHealth, _popup);
        _character.GetComponent<SFX>().Init(_sfxButton, _audioSettings);

        _movement.Init(_projectiles, _health, _camera);

        _side.Init(_health);
        _side.GetComponent<SFX>().Init(_sfxButton, _audioSettings);

        _healthView.Init(_health);

        _health.Init(_bombPlatform);

        _bombPlatform.Init(_sequence, _health, _enemyCharacter, _helper);

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

    public void Init(Character character, Character enemyCharacter, CinemachineVirtualCamera camera)
    {
        _character = character;
        _enemyCharacter = enemyCharacter;
        _camera = camera;
    }
}