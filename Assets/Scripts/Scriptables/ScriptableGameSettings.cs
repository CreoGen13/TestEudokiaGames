using UnityEngine;

namespace Scriptables
{
    [CreateAssetMenu(fileName = "Game Settings", menuName = "Scriptables/Game Settings", order = 1)]
    public class ScriptableGameSettings : ScriptableObject
    {
        [Header("Game")]
        public float spawnSupplyTime;
        public float spawnEnemyTime;
        public float stopSpawnEnemyTime;
        public float upgradeTime;
        public float gameCoefficient;
        
        [Header("Projectiles")]
        public float projectileAfterHitLifeTime;
        public float projectileLifeTime;
        public float projectileDamage;

        [Header("Enemies")]
        public string enemyTag;
        public float enemyHealth;
        public float enemySpeed;
        public float enemyDistanceTolerance;
        
        [Header("Supplies")]
        public string supplyTag;
        public Vector3 supplyRotation;
    }
}