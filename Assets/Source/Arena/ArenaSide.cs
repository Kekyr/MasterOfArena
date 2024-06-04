using System;
using DG.Tweening;
using UnityEngine;

public class ArenaSide : MonoBehaviour
{
    private readonly float NewScale = 0f;
    private readonly float Duration = 0.05f;

    [SerializeField] private Environment _environment;
    [SerializeField] private Explosion _explosion;
    [SerializeField] private SFX _sfx;
    [SerializeField] private SFXSO _blast;

    private Health _health;

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

        _health.Died += OnDead;
    }

    private void OnDisable()
    {
        _health.Died -= OnDead;
    }

    public void Init(Health health)
    {
        if (health == null)
        {
            throw new ArgumentNullException(nameof(health));
        }

        _health = health;
        enabled = true;
    }

    private void OnDead()
    {
        _environment.transform.DOScale(NewScale, Duration)
            .SetEase(Ease.InOutSine)
            .OnComplete(() =>
            {
                for (int i = 0; i < _explosion.transform.childCount; i++)
                {
                    _explosion.transform.GetChild(i).gameObject.SetActive(true);
                }

                _sfx.Play(_blast);
            });
    }
}