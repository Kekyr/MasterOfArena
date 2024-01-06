using System;
using DG.Tweening;
using UnityEngine;

public class Cube : MonoBehaviour
{
    private readonly float NewScale = 1f;
    private readonly float Duration = 0.05f;
    private readonly float Delay = 0.05f;

    [SerializeField] private uint _damage;
    [SerializeField] private CubeView _view;

    public event Action<Collision, Cube> Collided;

    public uint Damage => _damage;

    private void OnEnable()
    {
        if (_view == null)
            throw new ArgumentNullException(nameof(_view));

        _view.Init(_damage.ToString());

        transform.localScale = Vector3.zero;
        transform.DOScale(NewScale, Duration)
            .SetEase(Ease.InOutSine)
            .SetDelay(Delay);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Collided?.Invoke(collision, this);
    }
}