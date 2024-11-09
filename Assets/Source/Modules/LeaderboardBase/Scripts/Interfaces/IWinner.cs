using System;

namespace LeaderboardBase
{
    public interface IWinner
    {
        public event Action Victory;
    }
}