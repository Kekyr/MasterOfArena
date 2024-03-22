using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Popup : MonoBehaviour
{
    private readonly float NewScale = 1f;

    [SerializeField] private UpperPart _upperPart;
    [SerializeField] private LowerPart _lowerPart;

    [SerializeField] private float Duration = 1f;

    private Button _nextButton;

    private void OnEnable()
    {
        _upperPart.transform.localScale = Vector3.zero;
        _lowerPart.transform.localScale = Vector3.zero;

        _upperPart.transform.DOScale(NewScale, Duration)
            .SetEase(Ease.OutBounce)
            .OnComplete(() =>
            {
                _lowerPart.transform.DOScale(NewScale, Duration)
                    .SetEase(Ease.InOutSine);
            });

        _nextButton = _lowerPart.GetComponentInChildren<Button>();
        _nextButton.onClick.AddListener(Next);
    }

    private void OnDisable()
    {
        _nextButton.onClick.RemoveListener(Next);
    }

    private void Next()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}