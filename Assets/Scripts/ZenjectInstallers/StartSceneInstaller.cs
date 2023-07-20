using Cinemachine;
using Infrastructure.Pools.Enemy;
using Infrastructure.Pools.Projectile;
using Mono;
using Scriptables;
using UnityEngine;
using Zenject;

namespace ZenjectInstallers
{
    public class StartSceneInstaller : MonoInstaller
    {
        [Header("References")]
        [SerializeField] private Game game;
        [SerializeField] private CinemachineVirtualCamera virtualCamera;
        [SerializeField] private Camera playerCamera;
        [SerializeField] private Transform projectilePoolParent;
        [SerializeField] private Transform enemyPoolParent;
        
        [Header("Points")]
        [SerializeField] private Transform playerPoint;
        [SerializeField] private Transform [] enemyPoints;
        
        [Header("Prefabs")]
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private GameObject enemyPrefab;
        [SerializeField] private GameObject projectilePrefab;
        
        [Header("Scriptables")]
        [SerializeField] private ScriptablePlayerSettings playerSettings;
        [SerializeField] private ScriptableGameSettings gameSettings;
        public override void InstallBindings()
        {
            BindScriptables();
            BindProjectilePool();
            BindCamera();
            BindPlayer();
            BindEnemyPool();

            BindGame();
        }

        private void BindGame()
        {
            Container
                .Bind<Game>()
                .FromInstance(game)
                .AsSingle()
                .NonLazy();
        }
        private void BindCamera()
        {
            Container
                .Bind<Camera>()
                .FromInstance(playerCamera)
                .AsSingle();
        }
        private void BindPlayer()
        {
            PlayerController playerController =
                Container.InstantiatePrefabForComponent<PlayerController>(playerPrefab, playerPoint.position,
                    Quaternion.identity, null);

            virtualCamera.Follow = playerController.CameraFollow;

            Container
                .Bind<PlayerController>()
                .FromInstance(playerController)
                .AsSingle();
        }
        private void BindScriptables()
        {
            Container
                .Bind<ScriptablePlayerSettings>()
                .FromInstance(playerSettings)
                .AsSingle();
            
            Container
                .Bind<ScriptableGameSettings>()
                .FromInstance(gameSettings)
                .AsSingle();
        }
        private void BindProjectilePool()
        {
            Container
                .Bind<ProjectileFactory>()
                .AsSingle()
                .WithArguments(projectilePrefab);

            Container
                .Bind<IProjectilePool>()
                .To<ProjectilePool>()
                .AsCached()
                .WithArguments(projectilePoolParent);

            var pool = (ProjectilePool)Container.Resolve<IProjectilePool>();
            Container
                .Bind<ProjectilePool>()
                .FromInstance(pool)
                .AsCached()
                .WithArguments(projectilePoolParent);
        }
        private void BindEnemyPool()
        {
            Container
                .Bind<EnemyFactory>()
                .AsSingle()
                .WithArguments(enemyPrefab);

            Container
                .Bind<EnemyPool>()
                .AsSingle()
                .WithArguments(enemyPoolParent, enemyPoints);

        }
    }
}
