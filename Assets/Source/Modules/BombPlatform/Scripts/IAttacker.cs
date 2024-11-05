using System;
using UnityEngine;

public interface IAttacker
{
    public event Action<Vector3, uint> Attacked;
}