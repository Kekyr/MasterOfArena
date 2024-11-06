using System;
using Audio;
using DG.Tweening;
using UnityEngine;

public class ArenaSide : MonoBehaviour
{
    private readonly float _newScale = 0f;
    private readonly float _duration = 0.05f;

    [SerializeField] private Transform _environment;
    [SerializeField] private Transform _explosion;
    [SerializeField] private SFX _sfx;
    [SerializeField] private SFXSO _blast;
    [SerializeField] private Collider _catchZone;

    private Health _health;

    public Collider CatchZone => _catchZone;

    private void OnEnable()
    {
        if (_sfx == null)
        {
            throw new ArgumentNullException(nameof(_sfx));
        }

        if (_blast == null)
        {
            throw new ArgumentNullException(nameof(_blast));
        }

        if (_catchZone == null)
        {
            throw new ArgumentNullException(nameof(_catchZone));
        }
    }

    private void OnDisable()
    {
        _health.Died -= OnDead;
    }

    public void Init(Health health)
    {
        _health = health;
        _health.Died += OnDead;
        enabled = true;
    }

    private void OnDead()
    {
        _environment.DOScale(_newScale, _duration)
            .SetEase(Ease.InOutSine)
            .OnComplete(() =>
            {
                for (int i = 0; i < _explosion.childCount; i++)
                {
                    _explosion.GetChild(i).gameObject.SetActive(true);
                }

                _sfx.Play(_blast);
            });
    }
}