using System;
using UnityEngine;

public class Coins : MonoBehaviour
{
    private PlayerDataSO _data;

    public event Action<int> Added;

    private void OnEnable()
    {
        Added?.Invoke(_data.Coins);
    }

    public void Init(PlayerDataSO data)
    {
        if (data == null)
        {
            throw new ArgumentNullException(nameof(data));
        }

        _data = data;
        enabled = true;
    }

    public void AddCoins()
    {
        _data.AddCoins();
        Added?.Invoke(_data.Coins);
    }
}