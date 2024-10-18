using System;
using System.Collections.Generic;
using Lean.Localization;
using UnityEngine;
using YG;
using YG.Utils.LB;

public class YandexLeaderboard : MonoBehaviour
{
    public const string LeaderboardName = "FinalLeaderboard";
    private const string TranslationName = "AnonymPhrase";

    private readonly List<LeaderboardPlayer> _leaderboardPlayers = new();

    [SerializeField] private LeaderboardView _leaderboardView;

    private Player _player;
    private RewardedAd _rewardedAd;
    private LeanTranslation _translation;
    private PlayerDataSO _playerData;

    private bool _isButtonClicked;

    private void OnEnable()
    {
        if (_leaderboardView == null)
        {
            throw new ArgumentNullException(nameof(_leaderboardView));
        }

        _translation = LeanLocalization.GetTranslation(TranslationName);

        _player.Victory += SetPlayerScore;
        _rewardedAd.Rewarded += SetPlayerScore;
        YandexGame.onGetLeaderboard += OnGet;
    }

    private void OnDisable()
    {
        _player.Victory -= SetPlayerScore;
        _rewardedAd.Rewarded -= SetPlayerScore;
        YandexGame.onGetLeaderboard -= OnGet;
    }

    public void Init(Player player, RewardedAd rewardedAd, PlayerDataSO playerData)
    {
        _player = player;
        _rewardedAd = rewardedAd;
        _playerData = playerData;
        enabled = true;
    }

    public void SetPlayerScore()
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
        if (lb.thisPlayer.score < _playerData.Score)
        {
            YandexGame.NewLeaderboardScores(LeaderboardName, _playerData.Score);
        }
    }
}