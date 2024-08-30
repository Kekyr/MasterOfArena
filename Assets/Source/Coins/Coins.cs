using System;
using UnityEngine;

public class Coins : MonoBehaviour
{
    private PlayerDataSO _data;

    public event Action<int> Changed;

    private void OnEnable()
    {
        Changed?.Invoke(_data.Coins);
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

    public void Add()
    {
        _data.AddCoins();
        Changed?.Invoke(_data.Coins);
    }

    public bool TryRemove(int amount)
    {
        return _data.Coins - amount < 0;
    }

    public void Remove(int amount)
    {
        _data.RemoveCoins(amount);
        Changed?.Invoke(_data.Coins);
    }
}