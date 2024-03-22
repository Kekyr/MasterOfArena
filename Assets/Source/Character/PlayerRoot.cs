using System;
using UnityEngine;

public class PlayerRoot : CharacterRoot
{
    [SerializeField] private PlayerTargeting _targeting;

    private PlayerInputRouter _inputRouter;

    protected override void OnEnable()
    {
        base.OnEnable();

        if (_targeting == null)
            throw new ArgumentNullException(nameof(_targeting));
    }

    protected override void Start()
    {
        _targeting.Init(_inputRouter);
        base.Start();
    }

    public void Init(PlayerInputRouter inputRouter)
    {
        _inputRouter = inputRouter;
    }
}