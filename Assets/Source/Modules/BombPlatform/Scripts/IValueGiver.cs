using System;

public interface IValueGiver
{
    public event Action<float> ValueChanged;
}