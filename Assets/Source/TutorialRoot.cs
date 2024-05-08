using System;
using UnityEngine;

public class TutorialRoot : Root
{
    [SerializeField] private TutorialHand _hand;

    protected override void Validate()
    {
        base.Validate();

        if (_hand == null)
        {
            throw new ArgumentNullException(nameof(_hand));
        }
    }

    protected override void Awake()
    {
        base.Awake();

        _hand.Init(Order, InputRouter);
    }
}