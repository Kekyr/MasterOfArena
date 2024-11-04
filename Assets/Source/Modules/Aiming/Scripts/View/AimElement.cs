using DG.Tweening;
using UnityEngine;

public class AimElement : MonoBehaviour
{
    private readonly float _duration = 0.05f;
    private readonly float _minScale = 0f;

    [SerializeField] private float _maxScale;

    private Health _health;
    private Character _character;
    private Targeting _targeting;

    protected virtual void OnEnable()
    {
        transform.localScale = Vector3.zero;
        _character.Aimed += DecreaseScale;
        _targeting.Aiming += IncreaseScale;
    }

    protected virtual void OnDisable()
    {
        _character.Aimed -= DecreaseScale;
        _character.Won -= DecreaseScale;
        _targeting.Aiming -= IncreaseScale;
        _health.Died -= DecreaseScale;
    }

    public void Init(Character character, Targeting targeting, Health health)
    {
        _character = character;
        _targeting = targeting;
        _health = health;

        _health.Died += DecreaseScale;
        _character.Won += DecreaseScale;

        enabled = true;
    }

    protected void IncreaseScale()
    {
        ChangeScale(_maxScale);
    }

    private void DecreaseScale()
    {
        ChangeScale(_minScale);
    }

    private void ChangeScale(float endValue)
    {
        transform.DOScale(endValue, _duration)
            .SetEase(Ease.InOutSine);
    }
}