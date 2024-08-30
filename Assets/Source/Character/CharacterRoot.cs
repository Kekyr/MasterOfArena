using System;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using UnityEngine;

public class CharacterRoot : MonoBehaviour
{
    [SerializeField] private Popup _popup;
    [SerializeField] private Helper _helper;

    private ArenaSide _side;
    private BombPlatform _platformWithBomb;

    private ParticleSystem _confettiVFX;
    private CinemachineVirtualCamera _winCamera;

    private Character _character;
    private Character _enemyCharacter;

    private HealthView _healthView;
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

    public Popup Window => _popup;

    public IReadOnlyCollection<Projectile> Projectiles => _projectiles;

    public Targeting Aiming => _aiming;

    public Character Person => _character;

    public BombPlatform PlatformWithBomb => _platformWithBomb;

    protected virtual void OnEnable()
    {
        if (_popup == null)
        {
            throw new ArgumentNullException(nameof(_popup));
        }

        if (_helper == null)
        {
            throw new ArgumentNullException(nameof(_helper));
        }

        _platformWithBomb = _side.GetComponentInChildren<BombPlatform>();
        _confettiVFX = _side.GetComponentInChildren<ParticleSystem>();
        _winCamera = _side.GetComponentInChildren<CinemachineVirtualCamera>();
        _winCamera.gameObject.SetActive(false);
        _healthView = _side.GetComponentInChildren<HealthView>();

        _projectiles = _character.GetComponentsInChildren<Projectile>();
        _movement = _character.GetComponent<Movement>();

        _aiming = _character.GetComponent<Targeting>();
        _arrow = _character.GetComponentInChildren<Arrow>();
        _circle = _character.GetComponentInChildren<Circle>();
    }

    protected virtual void Start()
    {
        for (int i = 0; i < _projectiles.Length; i++)
        {
            _projectiles[i].Init(_character, _helper);
            _projectiles[i].GetComponent<SFX>().Init(_sfxButton, _audioSettings);
        }

        _character.Init(_confettiVFX, _winCamera);
        _character.Init(_projectiles, _popup);
        _character.GetComponent<SFX>().Init(_sfxButton, _audioSettings);

        _movement.Init(_projectiles, _health, _camera);

        _side.Init(_health);
        _side.GetComponent<SFX>().Init(_sfxButton, _audioSettings);

        _healthView.Init(_health);

        _health.Init(_platformWithBomb);

        _platformWithBomb.Init(_sequence, _health, _enemyCharacter, _helper);

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
    }

    public void Init(Character character, Character enemyCharacter, CinemachineVirtualCamera camera)
    {
        _character = character;
        _enemyCharacter = enemyCharacter;
        _camera = camera;
    }

    public void Init(Health health, ArenaSide side)
    {
        _health = health;
        _side = side;

        _character.Init(_health, _sequence);
    }

    public void Init(Health enemyHealth)
    {
        _enemyHealth = enemyHealth;

        _character.Init(_enemyHealth);
        enabled = true;
    }
}