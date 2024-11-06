using System;

namespace LeaderboardBase
{
    public interface IRewarder
    {
        public event Action Rewarded;
    }
}