using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TutorialHand : MonoBehaviour
{
    private readonly int MaxTimeScale = 1;
    private readonly int MinTimeScale = 0;

    private readonly int LoopsCount = -1;
    private readonly float Duration = 2;

    [SerializeField] private RectTransform[] _wayPoints;
    [SerializeField] private Button _leaderboardButton;
    [SerializeField] private Button _settingsButton;

    [SerializeField] private int _resolution;
    [SerializeField] private float _tutorialTimeScale;

    private PlayerInputRouter _inputRouter;

    public float TutorialTimeScale => _tutorialTimeScale;

    private void OnEnable()
    {
        if (_tutorialTimeScale < MinTimeScale || _tutorialTimeScale > MaxTimeScale)
        {
            throw new ArgumentOutOfRangeException(nameof(_tutorialTimeScale));
        }

        if (_wayPoints.Length == 0)
        {
            throw new ArgumentOutOfRangeException(nameof(_wayPoints));
        }

        if (_leaderboardButton == null)
        {
            throw new ArgumentNullException(nameof(_leaderboardButton));
        }

        if (_settingsButton == null)
        {
            throw new ArgumentNullException(nameof(_settingsButton));
        }

        _leaderboardButton.interactable = false;
        _settingsButton.interactable = false;

        Vector3[] wayPoints = new Vector3[_wayPoints.Length];

        for (int i = 0; i < _wayPoints.Length; i++)
        {
            wayPoints[i] = _wayPoints[i].position;
        }

        Time.timeScale = _tutorialTimeScale;
        transform.DOPath(wayPoints, Duration, PathType.CatmullRom, PathMode.Ignore, _resolution, Color.red)
            .SetEase(Ease.InOutSine)
            .SetLoops(LoopsCount, LoopType.Restart);

        _inputRouter.Aiming.performed += ctx => Stop();
    }

    private void OnDisable()
    {
        _inputRouter.Aiming.performed -= ctx => Stop();
    }

    public void Init(PlayerInputRouter inputRouter)
    {
        if (inputRouter == null)
        {
            throw new ArgumentNullException(nameof(inputRouter));
        }

        _inputRouter = inputRouter;
        enabled = true;
    }

    private void Stop()
    {
        Time.timeScale = MaxTimeScale;
        _leaderboardButton.interactable = true;
        _settingsButton.interactable = true;
        gameObject.SetActive(false);
    }
}