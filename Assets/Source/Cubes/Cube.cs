using System;
using UnityEngine;

public class Cube : MonoBehaviour
{
    [SerializeField] private uint _damage;
    [SerializeField] private CubeView _view;

    private void OnEnable()
    {
        if (_view == null)
            throw new ArgumentNullException(nameof(_view));

        _view.Init(_damage.ToString());
    }

    public void Init()
    {
        enabled = true;
    }
}