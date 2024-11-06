using System;

namespace Aiming
{
    public interface IThrower
    {
        public event Action Aimed;

        public event Action Won;
    }
}