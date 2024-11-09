using System;
using LeaderboardBase;
using UnityEngine;
using YG;

namespace UI
{
    public class LeaderboardButton : MainButton
    {
        [SerializeField] private Leaderboard _leaderboard;

        protected override void OnEnable()
        {
            base.OnEnable();

            if (_leaderboard == null)
            {
                throw new ArgumentNullException(nameof(_leaderboard));
            }
        }

        protected override void OnClick()
        {
            if (YandexGame.auth == false)
            {
                base.OnClick();
                return;
            }

            _leaderboard.Fill();
        }
    }
}