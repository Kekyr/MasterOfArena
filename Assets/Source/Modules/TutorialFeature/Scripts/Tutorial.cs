using UnityEngine;

namespace TutorialFeature
{
    public class Tutorial : MonoBehaviour
    {
        private TutorialHand _hand;
        private TutorialSO _tutorialData;

        public void Init(TutorialSO tutorialData, TutorialHand hand)
        {
            _tutorialData = tutorialData;
            _hand = hand;

            if (_tutorialData.CanPlay)
            {
                _hand.gameObject.SetActive(true);
                _tutorialData.ChangeValue();
            }
        }
    }
}