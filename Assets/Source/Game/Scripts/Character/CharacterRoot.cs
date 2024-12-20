using System;
using System.Collections.Generic;
using Aiming;
using Arena;
using Audio;
using BombPlatformFeature;
using Cinemachine;
using DG.Tweening;
using Game;
using HealthSystem;
using ProjectileBase;
using UI;
using UnityEngine;
using IMortal = Aiming.IMortal;

namespace CharacterBase
{
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

        private AudioButton _sfxButton;
        private AudioSettingSO _sfxOptions;

        private Sequence _sequence;
        private CinemachineVirtualCamera _camera;

        private Collider _catchZone;

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
                _projectiles[i].Init(_character, _helper, _catchZone);
                _projectiles[i].GetComponent<SFX>().Init(_sfxButton, _sfxOptions);
            }

            _character.Init(_confettiVFX, _winCamera);
            _character.Init(_projectiles, _popup);
            _character.GetComponent<SFX>().Init(_sfxButton, _sfxOptions);

            _movement.Init(_projectiles, _health, _camera);

            _side.Init(_health);
            _side.GetComponent<SFX>().Init(_sfxButton, _sfxOptions);

            _healthView.Init(_health);

            _health.Init(_platformWithBomb);

            _platformWithBomb.Init(_sequence, _health, _enemyCharacter, _helper);

            List<IMortal> mortals = new List<IMortal>();
            mortals.Add(_health);
            mortals.Add(_enemyHealth);

            _aiming.Init(_sequence, mortals);
            _arrow.Init(_character, _aiming, _health);

            _circle.Init(_projectiles);
            _circle.Init(_character, _aiming, _health);
        }

        public void Init(Sequence sequence, AudioButton sfxButton, AudioSettingSO sfxOptions)
        {
            _sequence = sequence;
            _sfxButton = sfxButton;
            _sfxOptions = sfxOptions;
        }

        public void Init(Character character, Character enemyCharacter, CinemachineVirtualCamera camera)
        {
            _character = character;
            _enemyCharacter = enemyCharacter;
            _camera = camera;
        }

        public void Init(Health health, ArenaSide side, Collider catchZone)
        {
            _health = health;
            _side = side;
            _catchZone = catchZone;

            _character.Init(_health, _sequence);
        }

        public void Init(Health enemyHealth)
        {
            _enemyHealth = enemyHealth;

            _character.Init(_enemyHealth);
            enabled = true;
        }
    }
}