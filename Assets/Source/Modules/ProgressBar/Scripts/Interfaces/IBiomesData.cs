using UnityEngine;

namespace ProgressBarFeature
{
    public interface IBiomesData
    {
        public Sprite CurrentIcon { get; }

        public Sprite NextIcon { get; }

        public void Change();
    }
}