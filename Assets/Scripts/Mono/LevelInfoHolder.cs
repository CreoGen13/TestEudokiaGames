using UnityEngine;

namespace Mono
{
    public class LevelInfoHolder : MonoBehaviour
    {
        [SerializeField] private GameObject navMeshPlane;
        
        [Header("Points")]
        [SerializeField] private Transform playerPoint;
        [SerializeField] private Transform [] enemyPoints;
        [SerializeField] private Transform [] supplyPoints;
       
        public GameObject NavMeshPlane => navMeshPlane;
        public Transform  PlayerPoint => playerPoint;
        public Transform [] EnemyPoints => enemyPoints;
        public Transform [] SupplyPoints => supplyPoints;
        
    }
}