using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardElement : MonoBehaviour
{
    [SerializeField] private Image _playerAvatar;

    [SerializeField] private TMP_Text _playerName;
    [SerializeField] private TMP_Text _playerRank;
    [SerializeField] private TMP_Text _playerScore;

    private void OnEnable()
    {
        if (_playerAvatar == null)
            throw new ArgumentNullException(nameof(_playerAvatar));

        if (_playerName == null)
            throw new ArgumentNullException(nameof(_playerName));

        if (_playerRank == null)
            throw new ArgumentNullException(nameof(_playerRank));

        if (_playerScore == null)
            throw new ArgumentNullException(nameof(_playerScore));
    }

    public void Initialize(Sprite avatar, string name, int rank, int score)
    {
        _playerAvatar.sprite = avatar;
        _playerName.text = name;
        _playerRank.text = rank.ToString();
        _playerScore.text = score.ToString();
    }
}