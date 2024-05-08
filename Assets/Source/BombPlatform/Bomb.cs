using System;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] private MeshRenderer _meshRenderer;

    public MeshRenderer MeshRenderer => _meshRenderer;

    private void OnEnable()
    {
        if (_meshRenderer == null)
        {
            throw new ArgumentNullException(nameof(_meshRenderer));
        }
    }
}
