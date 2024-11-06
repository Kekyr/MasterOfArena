using UnityEngine;

namespace BombPlatformFeature
{
    public interface IMeshChanger
    {
        public void ChangeMeshColor(MeshRenderer meshRenderer, Color tempColor, float duration);
    }
}