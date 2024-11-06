using UnityEngine;

namespace Audio
{
    [CreateAssetMenu(fileName = "new ImageSO", menuName = "ImageSO/Create new ImageSO")]
    public class ImageSO : ScriptableObject
    {
        [SerializeField] private Sprite[] _sprites;

        private int _currentIndex;

        public Sprite CurrentSprite => _sprites[_currentIndex];

        public int CurrentIndex => _currentIndex;

        public void Init(int currentIndex)
        {
            _currentIndex = currentIndex;
        }

        public Sprite Switch()
        {
            _currentIndex++;

            if (_currentIndex > _sprites.Length - 1)
            {
                _currentIndex = 0;
            }

            return _sprites[_currentIndex];
        }
    }
}