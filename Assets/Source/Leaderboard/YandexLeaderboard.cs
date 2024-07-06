using System;
using System.Collections.Generic;
using Agava.YandexGames;
using Lean.Localization;
using UnityEngine;

public class YandexLeaderboard : MonoBehaviour
{
    public const string LeaderboardName = "NewNewLeaderboard";
    private const string TranslationName = "AnonymPhrase";

    private readonly List<LeaderboardPlayer> _leaderboardPlayers = new();

    [SerializeField] private LeaderboardView _leaderboardView;

    private Player _player;
    private RewardedAd _rewardedAd;
    private LeanTranslation _translation;

    private void OnEnable()
    {
        if (_leaderboardView == null)
        {
            throw new ArgumentNullException(nameof(_leaderboardView));
        }

        _translation = LeanLocalization.GetTranslation(TranslationName);

        _player.Victory += SetPlayerScore;
        _rewardedAd.Rewarded += SetPlayerScore;
    }

    private void OnDisable()
    {
        _player.Victory -= SetPlayerScore;
        _rewardedAd.Rewarded -= SetPlayerScore;
    }

    public void Init(Player player, RewardedAd rewardedAd)
    {
        if (player == null)
        {
            throw new ArgumentNullException(nameof(player));
        }

        if (rewardedAd == null)
        {
            throw new ArgumentNullException(nameof(rewardedAd));
        }

        _player = player;
        _rewardedAd = rewardedAd;
        enabled = true;
    }

    public void SetPlayerScore(int score)
    {
        if (PlayerAccount.IsAuthorized == false)
        {
            return;
        }

        Leaderboard.GetPlayerEntry(LeaderboardName, (result) =>
        {
            if (result == null || result.score < score)
            {
                Leaderboard.SetScore(LeaderboardName, score);
            }
        });
    }

    public void Fill()
    {
        if (PlayerAccount.IsAuthorized == false)
        {
            return;
        }

        _leaderboardPlayers.Clear();

        Leaderboard.GetEntries(LeaderboardName, LoadData);
    }

    private void LoadData(LeaderboardGetEntriesResponse result)
    {
        foreach (var entry in result.entries)
        {
            string id = entry.player.uniqueID;
            string avatar = entry.player.profilePicture;
            string name = entry.player.publicName;

            if (string.IsNullOrEmpty(name))
            {
                name = (string)_translation.Data;
            }

            int rank = entry.rank;
            int score = entry.score;

            _leaderboardPlayers.Add(new LeaderboardPlayer(id, avatar, name, rank, score));
        }

        _leaderboardView.ConstructLeaderboard(_leaderboardPlayers);
    }
}