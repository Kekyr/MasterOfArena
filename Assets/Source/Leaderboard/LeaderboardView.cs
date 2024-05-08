using System;
using System.Collections.Generic;
using Agava.YandexGames;
using UnityEngine;

public class LeaderboardView : MonoBehaviour
{
    private const string LeaderboardName = "NewLeaderboard";
    private const int Size = 5;

    [SerializeField] private Transform _container;
    [SerializeField] private Transform _playerScoreContainer;

    [SerializeField] private LeaderboardElement _leaderboardElementPrefab;
    [SerializeField] private LeaderboardElement _playerLeaderboardElementPrefab;

    private List<LeaderboardElement> _spawnedElements = new();

    private void OnEnable()
    {
        if (_container == null)
        {
            throw new ArgumentNullException(nameof(_container));
        }

        if (_playerScoreContainer == null)
        {
            throw new ArgumentNullException(nameof(_playerScoreContainer));
        }

        if (_leaderboardElementPrefab == null)
        {
            throw new ArgumentNullException(nameof(_leaderboardElementPrefab));
        }
    }

    public void ConstructLeaderboard(List<LeaderboardPlayer> leaderboardPlayers)
    {
        ClearLeaderboard();

#if UNITY_WEBGL && !UNITY_EDITOR
        Leaderboard.GetPlayerEntry(LeaderboardName, (result) =>
        {
            string playerId = result.player.uniqueID;
            int length = Size > leaderboardPlayers.Count ? leaderboardPlayers.Count : Size;

            for (int i = 0; i < length; i++)
            {
                if (leaderboardPlayers[i].Id == playerId)
                {
                    SpawnLeaderboardElement(_playerLeaderboardElementPrefab, leaderboardPlayers[i],
                        _playerScoreContainer);
                    SpawnLeaderboardElement(_playerLeaderboardElementPrefab, leaderboardPlayers[i], _container);
                }
                else
                {
                    SpawnLeaderboardElement(_leaderboardElementPrefab, leaderboardPlayers[i], _container);
                }
            }

            gameObject.SetActive(true);
        });
#endif
    }

    private void SpawnLeaderboardElement(LeaderboardElement prefab, LeaderboardPlayer player, Transform container)
    {
        LeaderboardElement leaderboardElementInstance = Instantiate(prefab, container);
        leaderboardElementInstance.Initialize(player.Avatar, player.Name, player.Rank, player.Score);
        _spawnedElements.Add(leaderboardElementInstance);
    }

    private void ClearLeaderboard()
    {
        foreach (var element in _spawnedElements)
        {
            Destroy(element.gameObject);
        }

        _spawnedElements = new List<LeaderboardElement>();
    }
}