using System;
using Agava.YandexGames;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardElement : MonoBehaviour
{
    private const float PivotX = 0.5f;
    private const float PivotY = 0.5f;

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

    public void Initialize(string avatar, string name, int rank, int score)
    {
        RemoteImage image = new RemoteImage(avatar);
        image.Download(OnSuccessCallback, OnErrorCallback);

        _playerName.text = name;
        _playerRank.text = rank.ToString();
        _playerScore.text = score.ToString();
    }

    private void OnSuccessCallback(Texture2D texture)
    {
        _playerAvatar.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height),
            new Vector2(PivotX, PivotY));
    }

    private void OnErrorCallback(string error)
    {
        Debug.Log(error);
    }
}