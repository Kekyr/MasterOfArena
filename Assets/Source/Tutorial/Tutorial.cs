using System;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    private TutorialHand _hand;
    private TutorialSO _tutorialData;

    private void OnEnable()
    {
        if (_tutorialData.CanPlay)
        {
            _hand.gameObject.SetActive(true);
            _tutorialData.ChangeValue();
        }
    }

    public void Init(TutorialSO tutorialData, TutorialHand hand)
    {
        if (tutorialData == null)
        {
            throw new ArgumentNullException(nameof(tutorialData));
        }

        if (hand == null)
        {
            throw new ArgumentNullException(nameof(hand));
        }

        _tutorialData = tutorialData;
        _hand = hand;
        enabled = true;
    }
}