using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TutorialHand : MonoBehaviour
{
    private readonly int _maxTimeScale = 1;
    private readonly int _minTimeScale = 0;

    private readonly int _loopsCount = -1;
    private readonly float _duration = 2;

    private readonly float _minCoinsViewAlpha = 0.5f;
    private readonly float _maxCoinsViewAlpha = 1f;

    [SerializeField] private RectTransform[] _wayPoints;

    [SerializeField] private int _resolution;
    [SerializeField] private float _tutorialTimeScale;

    private PlayerInputRouter _inputRouter;
    private Button[] _mainButtons;

    private CoinsView _coinsView;
    private CanvasGroup _coinsViewGroup;

    public float TutorialTimeScale => _tutorialTimeScale;

    private void OnEnable()
    {
        if (_tutorialTimeScale < _minTimeScale || _tutorialTimeScale > _maxTimeScale)
        {
            throw new ArgumentOutOfRangeException(nameof(_tutorialTimeScale));
        }

        if (_wayPoints.Length == 0)
        {
            throw new ArgumentOutOfRangeException(nameof(_wayPoints));
        }

        _coinsViewGroup = _coinsView.GetComponent<CanvasGroup>();
        _coinsViewGroup.alpha = _minCoinsViewAlpha;

        foreach (Button mainButton in _mainButtons)
        {
            mainButton.interactable = false;
        }

        Vector3[] wayPoints = new Vector3[_wayPoints.Length];

        for (int i = 0; i < _wayPoints.Length; i++)
        {
            wayPoints[i] = _wayPoints[i].position;
        }

        Time.timeScale = _tutorialTimeScale;
        transform.DOPath(wayPoints, _duration, PathType.CatmullRom, PathMode.Ignore, _resolution, Color.red)
            .SetEase(Ease.InOutSine)
            .SetLoops(_loopsCount, LoopType.Restart);

        _inputRouter.Aiming.performed += Stop;
    }

    private void OnDisable()
    {
        _inputRouter.Aiming.performed -= Stop;
    }

    public void Init(PlayerInputRouter inputRouter, Button[] mainButtons, CoinsView coinsView)
    {
        _inputRouter = inputRouter;
        _mainButtons = mainButtons;
        _coinsView = coinsView;
        enabled = true;
    }

    private void Stop(InputAction.CallbackContext context)
    {
        Time.timeScale = _maxTimeScale;

        _coinsViewGroup.alpha = _maxCoinsViewAlpha;

        foreach (Button mainButton in _mainButtons)
        {
            mainButton.interactable = true;
        }

        gameObject.SetActive(false);
    }
}