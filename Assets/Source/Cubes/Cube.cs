using DG.Tweening;
using System;
using UnityEngine;

public class Cube : MonoBehaviour
{
    [SerializeField] private uint _damage;
    [SerializeField] private CubeView _view;

    public event Action<Collision, Cube> Collided;

    public uint Damage => _damage;

    private void OnEnable()
    {
        if (_view == null)
            throw new ArgumentNullException(nameof(_view));

        _view.Init(_damage.ToString());

        transform.DOScale(1f, 0.05f)
            .SetEase(Ease.InOutSine)
            .SetDelay(0.05f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Collided?.Invoke(collision, this);
    }
}