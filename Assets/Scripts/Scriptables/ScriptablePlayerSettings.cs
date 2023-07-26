using UnityEngine;

namespace Scriptables
{
    [CreateAssetMenu(fileName = "Player Settings", menuName = "Scriptables/Player Settings", order = 2)]
    public class ScriptablePlayerSettings : ScriptableObject
    {
        [Header("Player")]
        public float playerRotationSpeed;
        
        [Header("Cannon")]
        public float projectileForwardForce;
        public float projectileUpForce;
        public float cannonReloadTime;
    }
}