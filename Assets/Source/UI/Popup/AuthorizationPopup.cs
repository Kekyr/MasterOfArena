using System;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class AuthorizationPopup : MainPopup
{
    [SerializeField] private Button _button;

    private YandexLeaderboard _leaderboard;

    protected override void OnEnable()
    {
        base.OnEnable();

        if (_button == null)
        {
            throw new ArgumentNullException(nameof(_button));
        }

        _button.onClick.AddListener(SignIn);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _button.onClick.RemoveListener(SignIn);
    }

    public void Init(YandexLeaderboard leaderboard)
    {
        _leaderboard = leaderboard;
    }

    private void SignIn()
    {
        if (YandexGame.auth == false)
        {
            YandexGame.AuthDialog();
        }
        else
        {
            Time.timeScale = 1f;
            gameObject.SetActive(false);
            _leaderboard.Fill();
        }
    }
}