using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Arena
{
    public class Props : MonoBehaviour
    {
        [SerializeField] private Transform[] _props;

        private void OnEnable()
        {
            if (_props.Length == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(_props));
            }

            int randomPropsIndex = Random.Range(0, _props.Length);

            _props[randomPropsIndex].gameObject.SetActive(true);
        }
    }
}
