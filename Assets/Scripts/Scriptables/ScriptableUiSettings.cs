using UnityEngine;

namespace Scriptables
{
    [CreateAssetMenu(fileName = "UI Settings", menuName = "Scriptables/UI Settings", order = 3)]
    public class ScriptableUiSettings : ScriptableObject
    {
        [Header("Animations")]
        public float openAnimationDuration;
        public float closeAnimationDuration;
        public float fadeAnimationDuration;

        [Header("Color")]
        public Color backgroundFadedColor;
    }
}