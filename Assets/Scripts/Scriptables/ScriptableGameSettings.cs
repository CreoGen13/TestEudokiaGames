using UnityEngine;

namespace Scriptables
{
    [CreateAssetMenu(fileName = "Game Settings", menuName = "Scriptables/Game Settings", order = 1)]
    public class ScriptableGameSettings : ScriptableObject
    {
        [Header("Projectiles")]
        public string enemyTag;
        public float projectileLifeTime;
    }
}