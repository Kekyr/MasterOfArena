using System;
using DG.Tweening;
using UnityEngine;

public class ArenaSide : MonoBehaviour
{
    private readonly float NewScale = 0f;
    private readonly float Duration = 0.05f;

    [SerializeField] private Environment _environment;
    [SerializeField] private Explosion _explosion;

    private Health _health;

    private void OnEnable()
    {
        if (_environment == null)
            throw new ArgumentNullException(nameof(_environment));

        if (_explosion == null)
            throw new ArgumentNullException(nameof(_explosion));

        _health.Died += OnDead;
    }

    private void OnDisable()
    {
        _health.Died -= OnDead;
    }

    public void Init(Health health)
    {
        if (health == null)
            throw new ArgumentNullException(nameof(health));

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
            });
    }
}