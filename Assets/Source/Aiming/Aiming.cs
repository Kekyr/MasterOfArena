using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Aiming : MonoBehaviour
{
    private readonly float DistanceFromCamera = 19.8f;
    private readonly float RotationX = 90;

    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private uint _speed;

    private Player _player;
    private Projectile _projectile;

    private bool canAim;

    public event Action<Vector3> Aimed;

    private void OnEnable()
    {
        if (_canvasGroup == null)
            throw new ArgumentNullException(nameof(_canvasGroup));

        Aimed += _player.OnAimed;
    }

    private void OnDisable()
    {
        Aimed -= _player.OnAimed;
    }

    public void Init(Player player, Projectile projectile)
    {
        if (player == null)
            throw new ArgumentNullException(nameof(player));

        if (projectile == null)
            throw new ArgumentNullException(nameof(projectile));

        _player = player;
        _projectile = projectile;
        enabled = true;
    }

    public void OnAimingStarted()
    {
        if (_projectile.IsFlying == false)
        {
            _canvasGroup.alpha = 1f;
            canAim = true;
            StartCoroutine(Aim());
        }
    }

    public void OnAimingCanceled()
    {
        canAim = false;
    }

    private IEnumerator Aim()
    {
        Vector3 throwDirection = Vector3.zero;

        while (canAim)
        {
            Vector3 mouseScreenPosition = Mouse.current.position.ReadValue();
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(mouseScreenPosition.x, mouseScreenPosition.y, DistanceFromCamera));

            mousePosition = new Vector3(mousePosition.x, transform.position.y, mousePosition.z);

            throwDirection = -(mousePosition - transform.position).normalized;
            Quaternion newRotation = Quaternion.LookRotation(throwDirection, Vector3.up);

            _player.transform.rotation = newRotation;

            Vector3 eulerAngles = newRotation.eulerAngles;
            eulerAngles.x = RotationX;

            transform.eulerAngles = eulerAngles;

            yield return null;
        }

        StartCoroutine(LerpAlpha(_canvasGroup, 0));

        Aimed?.Invoke(throwDirection);
    }

    private IEnumerator LerpAlpha(CanvasGroup canvasGroup, float alpha)
    {
        float newAlpha = canvasGroup.alpha;

        while (canvasGroup.alpha != alpha)
        {
            newAlpha = Mathf.MoveTowards(newAlpha, alpha, _speed * Time.deltaTime);
            canvasGroup.alpha = newAlpha;
            yield return null;
        }
    }
}