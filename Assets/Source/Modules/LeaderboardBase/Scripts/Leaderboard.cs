using System;
using System.Collections.Generic;
using Lean.Localization;
using UnityEngine;
using YG;
using YG.Utils.LB;

namespace LeaderboardBase
{
    public class Leaderboard : MonoBehaviour
    {
        private const string LeaderboardName = "FinalLeaderboard";
        private const string TranslationName = "AnonymPhrase";

        private readonly List<LeaderboardPlayer> _leaderboardPlayers = new();

        [SerializeField] private LeaderboardView _leaderboardView;

        private IWinner _winner;
        private IRewarder _rewarder;
        private LeanTranslation _translation;
        private ILeaderboardData _data;

        private bool _isButtonClicked;

        private void OnEnable()
        {
            if (_leaderboardView == null)
            {
                throw new ArgumentNullException(nameof(_leaderboardView));
            }

            _translation = LeanLocalization.GetTranslation(TranslationName);

            _winner.Victory += SetWinnerScore;
            _rewarder.Rewarded += SetWinnerScore;
            YandexGame.onGetLeaderboard += OnGet;
        }

        private void OnDisable()
        {
            _winner.Victory -= SetWinnerScore;
            _rewarder.Rewarded -= SetWinnerScore;
            YandexGame.onGetLeaderboard -= OnGet;
        }

        public void Init(IWinner player, IRewarder rewardedAd, ILeaderboardData data)
        {
            _winner = player;
            _rewarder = rewardedAd;
            _data = data;
            enabled = true;
        }

        public void SetWinnerScore()
        {
            if (YandexGame.auth == false)
            {
                return;
            }

            YandexGame.GetLeaderboard(LeaderboardName, 10, 3, 3, "medium");
        }

        public void Fill()
        {
            if (YandexGame.auth == false)
            {
                return;
            }

            _leaderboardPlayers.Clear();
            _isButtonClicked = true;
            YandexGame.GetLeaderboard(LeaderboardName, 10, 3, 3, "medium");
        }

        private void OnGet(LBData lb)
        {
            if (lb.technoName != LeaderboardName)
            {
                return;
            }

            TryChangeScore(lb);

            if (_isButtonClicked == true)
            {
                CreateLeaderboard(lb);
                _isButtonClicked = false;
            }
        }

        private void CreateLeaderboard(LBData lb)
        {
            foreach (var playerData in lb.players)
            {
                string id = playerData.uniqueID;
                string avatar = playerData.photo;
                string name = playerData.name;

                if (string.IsNullOrEmpty(name))
                {
                    name = (string)_translation.Data;
                }

                int rank = playerData.rank;
                int score = playerData.score;

                _leaderboardPlayers.Add(new LeaderboardPlayer(id, avatar, name, rank, score));
            }

            _leaderboardView.ConstructLeaderboard(_leaderboardPlayers);
        }

        private void TryChangeScore(LBData lb)
        {
            if (lb.thisPlayer.score < _data.Score)
            {
                YandexGame.NewLeaderboardScores(LeaderboardName, _data.Score);
            }
        }
    }
}