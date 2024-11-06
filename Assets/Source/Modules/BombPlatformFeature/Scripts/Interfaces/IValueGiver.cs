using System;

namespace BombPlatformFeature
{
    public interface IValueGiver
    {
        public event Action<float> ValueChanged;
    }
}