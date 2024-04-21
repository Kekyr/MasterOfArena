using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using Sequence = DG.Tweening.Sequence;

public class TutorialHand : MonoBehaviour
{
    private readonly int MaxTimeValue = 1;
    private readonly int MinTimeValue = 0;

    private readonly int LoopsCount = -1;
    private readonly float Duration = 2;

    [SerializeField] private int _resolution;
    [SerializeField] private float _newTimeValue;
    [SerializeField] private RectTransform[] _wayPoints;
    [SerializeField] private LeaderboardButton _leaderboardButton;
    [SerializeField] private SettingsButton _settingsButton;

    private Sequence _sequence;
    private PlayerInputRouter _inputRouter;

    private void OnEnable()
    {
        if (_newTimeValue < MinTimeValue || _newTimeValue > MaxTimeValue)
            throw new ArgumentOutOfRangeException(nameof(_newTimeValue));

        if (_wayPoints.Length == 0)
            throw new ArgumentOutOfRangeException(nameof(_wayPoints));

        if (_leaderboardButton == null)
            throw new ArgumentNullException(nameof(_leaderboardButton));

        if (_settingsButton == null)
            throw new ArgumentNullException(nameof(_settingsButton));

        Vector3[] wayPoints = new Vector3[_wayPoints.Length];

        for (int i = 0; i < _wayPoints.Length; i++)
            wayPoints[i] = _wayPoints[i].position;

        _sequence.OnComplete(() =>
        {
            Time.timeScale = _newTimeValue;
            transform.DOPath(wayPoints, Duration, PathType.CatmullRom, PathMode.Ignore, _resolution, Color.red)
                .SetEase(Ease.InOutSine)
                .SetLoops(LoopsCount, LoopType.Restart);
        });

        _inputRouter.Aiming.performed += ctx => Stop();
    }

    private void OnDisable()
    {
        _inputRouter.Aiming.performed -= ctx => Stop();
    }

    public void Init(Sequence sequence, PlayerInputRouter inputRouter)
    {
        if (sequence == null)
            throw new ArgumentNullException(nameof(_sequence));

        if (inputRouter == null)
            throw new ArgumentNullException(nameof(inputRouter));

        _sequence = sequence;
        _inputRouter = inputRouter;
        enabled = true;
    }

    private void Stop()
    {
        Time.timeScale = MaxTimeValue;
        _leaderboardButton.gameObject.SetActive(true);
        _settingsButton.gameObject.SetActive(true);
        gameObject.SetActive(false);
        enabled = false;
    }
}