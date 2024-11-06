using System;

namespace Aiming
{
    public interface IMortal
    {
        public event Action Died;
    }
}