using System;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardView : MonoBehaviour
{
    [SerializeField] private Transform _container;
    [SerializeField] private LeaderboardElement _leaderboardElementPrefab;

    private List<LeaderboardElement> _spawnedElements = new();

    private void OnEnable()
    {
        if (_container == null)
            throw new ArgumentNullException(nameof(_container));

        if (_leaderboardElementPrefab == null)
            throw new ArgumentNullException(nameof(_leaderboardElementPrefab));
    }

    public void ConstructLeaderboard(List<LeaderboardPlayer> leaderboardPlayers)
    {
        ClearLeaderboard();

        foreach (LeaderboardPlayer player in leaderboardPlayers)
        {
            LeaderboardElement leaderboardElementInstance = Instantiate(_leaderboardElementPrefab, _container);
            leaderboardElementInstance.Initialize(player.Avatar, player.Name, player.Rank, player.Score);

            _spawnedElements.Add(leaderboardElementInstance);
        }

        gameObject.SetActive(true);
    }

    private void ClearLeaderboard()
    {
        foreach (var element in _spawnedElements)
            Destroy(element.gameObject);

        _spawnedElements = new List<LeaderboardElement>();
    }
}