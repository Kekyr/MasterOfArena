using System;

namespace Audio
{
    public interface IMortal
    {
        public event Action Died;
    }
}