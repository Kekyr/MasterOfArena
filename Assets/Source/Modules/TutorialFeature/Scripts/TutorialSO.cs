using UnityEngine;

namespace TutorialFeature
{
    [CreateAssetMenu(fileName = "new TutorialSO", menuName = "TutorialSO/Create new TutorialSO")]
    public class TutorialSO : ScriptableObject
    {
        [SerializeField] private bool _canPlay;

        public bool CanPlay => _canPlay;

        public void Init(bool canPlay)
        {
            _canPlay = canPlay;
        }

        public void ChangeValue()
        {
            _canPlay = false;
        }
    }
}