using System;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TutorialHand : MonoBehaviour
{
    private readonly int MaxTimeScale = 1;
    private readonly int MinTimeScale = 0;

    private readonly int LoopsCount = -1;
    private readonly float Duration = 2;

    private readonly int StartAlpha = 100;
    private readonly int EndAlpha = 255;

    [SerializeField] private RectTransform[] _wayPoints;

    [SerializeField] private int _resolution;
    [SerializeField] private float _tutorialTimeScale;

    private PlayerInputRouter _inputRouter;
    private Button[] _mainButtons;

    private CoinsView _coinsView;
    private TextMeshProUGUI _coinsText;
    private Image _coinsIcon;

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

        foreach (Button mainButton in _mainButtons)
        {
            mainButton.interactable = false;
        }

        _coinsText = _coinsView.GetComponentInChildren<TextMeshProUGUI>();
        _coinsIcon = _coinsView.GetComponentInChildren<Image>();

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

    public void Init(PlayerInputRouter inputRouter, Button[] mainButtons, CoinsView coinsView)
    {
        if (inputRouter == null)
        {
            throw new ArgumentNullException(nameof(inputRouter));
        }

        if (mainButtons == null)
        {
            throw new ArgumentNullException(nameof(mainButtons));
        }

        if (coinsView == null)
        {
            throw new ArgumentNullException(nameof(coinsView));
        }

        _inputRouter = inputRouter;
        _mainButtons = mainButtons;
        _coinsView = coinsView;
        enabled = true;
    }

    private void Stop()
    {
        Time.timeScale = MaxTimeScale;

        foreach (Button mainButton in _mainButtons)
        {
            mainButton.interactable = true;
        }

        gameObject.SetActive(false);
    }
}