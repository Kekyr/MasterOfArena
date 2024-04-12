using System;
using System.Collections.Generic;
using Agava.YandexGames;
using UnityEngine;

public class YandexLeaderboard : MonoBehaviour
{
    private const string LeaderboardName = "Leaderboard";
    private const string AnonymousName = "Anonymous";

    private readonly List<LeaderboardPlayer> _leaderboardPlayers = new();

    [SerializeField] private LeaderboardView _leaderboardView;

    private void OnEnable()
    {
        if (_leaderboardView == null)
            throw new ArgumentNullException(nameof(_leaderboardView));
    }

    public void SetPlayerScore(int score)
    {
        if (PlayerAccount.IsAuthorized == false)
            return;

        Leaderboard.GetPlayerEntry(LeaderboardName, (result) =>
        {
            if (result == null || result.score < score)
                Leaderboard.SetScore(LeaderboardName, score);
        });
    }

    public void Fill()
    {
        if (PlayerAccount.IsAuthorized == false)
            return;

        _leaderboardPlayers.Clear();

        Leaderboard.GetEntries(LeaderboardName, (result) => { LoadData(result); });
    }

    private void LoadData(LeaderboardGetEntriesResponse result)
    {
        foreach (var entry in result.entries)
        {
            string avatar = entry.player.profilePicture;
            string name = entry.player.publicName;

            if (string.IsNullOrEmpty(name))
                name = AnonymousName;

            int rank = entry.rank;
            int score = entry.score;

            _leaderboardPlayers.Add(new LeaderboardPlayer(avatar, name, rank, score));
        }

        _leaderboardView.ConstructLeaderboard(_leaderboardPlayers);
    }
}