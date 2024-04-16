using System;
using System.Collections.Generic;
using Agava.YandexGames;
using UnityEngine;
using UnityEngine.Serialization;

public class LeaderboardView : MonoBehaviour
{
    private const string LeaderboardName = "Leaderboard";

    [SerializeField] private Transform _container;
    [SerializeField] private Transform _playerScoreContainer;

    [SerializeField] private LeaderboardElement _leaderboardElementPrefab;
    [SerializeField] private LeaderboardElement _playerLeaderboardElementPrefab;

    private List<LeaderboardElement> _spawnedElements = new();

    private void OnEnable()
    {
        if (_container == null)
            throw new ArgumentNullException(nameof(_container));

        if (_playerScoreContainer == null)
            throw new ArgumentNullException(nameof(_playerScoreContainer));

        if (_leaderboardElementPrefab == null)
            throw new ArgumentNullException(nameof(_leaderboardElementPrefab));
    }

    public void ConstructLeaderboard(List<LeaderboardPlayer> leaderboardPlayers)
    {
        ClearLeaderboard();

        LeaderboardPlayer authorizedPlayer = null;
        LeaderboardElement leaderboardElementInstance;

        Leaderboard.GetPlayerEntry(LeaderboardName, (result) =>
        {
            authorizedPlayer = new LeaderboardPlayer(result.player.uniqueID, result.player.profilePicture,
                result.player.publicName, result.rank, result.score);

            leaderboardElementInstance =
                Instantiate(_leaderboardElementPrefab, _playerScoreContainer);

            leaderboardElementInstance.Initialize(authorizedPlayer.Avatar, authorizedPlayer.Name, authorizedPlayer.Rank,
                authorizedPlayer.Score);

            _spawnedElements.Add(leaderboardElementInstance);

            foreach (LeaderboardPlayer player in leaderboardPlayers)
            {
                LeaderboardElement leaderboardElementInstance;

                if (player.Id == authorizedPlayer.Id)
                {
                    leaderboardElementInstance = Instantiate(_playerLeaderboardElementPrefab, _container);
                }
                else
                {
                    leaderboardElementInstance = Instantiate(_leaderboardElementPrefab, _container);
                }

                leaderboardElementInstance.Initialize(player.Avatar, player.Name, player.Rank, player.Score);

                _spawnedElements.Add(leaderboardElementInstance);
            }

            gameObject.SetActive(true);
        });
    }

    private void ClearLeaderboard()
    {
        foreach (var element in _spawnedElements)
            Destroy(element.gameObject);

        _spawnedElements = new List<LeaderboardElement>();
    }
}