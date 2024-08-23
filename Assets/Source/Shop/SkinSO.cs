using UnityEngine;

[CreateAssetMenu(fileName = "new SkinSO", menuName = "SkinSO/Create new SkinSO")]
public class SkinSO : ScriptableObject
{
    [SerializeField] private Sprite _sprite;
    [SerializeField] private int _cost;
    [SerializeField] private State _state;

    public Sprite Image => _sprite;

    public int Cost => _cost;

    public State Status => _state;

    public void UpgradeStatus()
    {
        if (_state == State.Selected)
        {
            return;
        }

        _state++;
    }

    public void DowngradeStatus()
    {
        if (_state == State.NotBought)
        {
            return;
        }

        _state--;
    }
}

public enum State
{
    NotBought,
    Bought,
    Selected
}