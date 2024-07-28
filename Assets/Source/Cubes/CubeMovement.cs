using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CubeMovement : MonoBehaviour
{
    [SerializeField] private float _yModifier;
    [SerializeField] private float _xModifier;
    [SerializeField] private float _duration;

    private Rigidbody _rigidbody;

    private void OnEnable()
    {
        _rigidbody = GetComponent<Rigidbody>();

        Vector3 endPosition = new Vector3(transform.position.x + _xModifier, transform.position.y + _yModifier,
            transform.position.z);

        _rigidbody.DOMove(endPosition, _duration)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.Linear);
    }
}