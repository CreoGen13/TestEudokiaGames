using Cinemachine;
using Enemy;
using Game;
using Infrastructure.Pools.Enemy;
using Infrastructure.Pools.Projectile;
using Infrastructure.Pools.Supply;
using Mono;
using Player;
using Scriptables;
using UI.Hud;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace ZenjectInstallers
{
    public class StartSceneInstaller : MonoInstaller
    {
        [Header("References")]
        [SerializeField] private CinemachineVirtualCamera virtualCamera;
        [SerializeField] private Camera playerCamera;
        [SerializeField] private Transform projectilePoolParent;
        [SerializeField] private Transform enemyPoolParent;
        [SerializeField] private Transform supplyPoolParent;
        [SerializeField] private LevelInfoHolder levelInfoHolder;
        [SerializeField] private GameView gameView;
        
        [Header("UI")]
        [SerializeField] private HudView hudView;

        [Header("Prefabs")]
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private GameObject enemyPrefab;
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private GameObject supplyPrefab;
        
        [Header("Scriptables")]
        [SerializeField] private ScriptablePlayerSettings playerSettings;
        [SerializeField] private ScriptableGameSettings gameSettings;
        public override void InstallBindings()
        {
            BindHud();
            BindScriptables();
            
            BindEnemyPool();
            BindSupplyPool();
            BindProjectilePool();
            
            BindCamera();
            BindPlayer();
            BindNavMeshHolder();

            BindGame();
        }

        private void BindGame()
        {
            Container
                .Bind<GamePresenter>()
                .AsSingle()
                .WithArguments(new GameModel(), gameView);
        }
        private void BindNavMeshHolder()
        {
            Container
                .Bind<LevelInfoHolder>()
                .FromInstance(levelInfoHolder)
                .AsSingle();
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
            PlayerView playerView =
                Container.InstantiatePrefabForComponent<PlayerView>(playerPrefab, levelInfoHolder.PlayerPoint.position,
                    Quaternion.identity, null);

            virtualCamera.Follow = playerView.CameraFollow;

            Container
                .Bind<PlayerPresenter>()
                .AsSingle()
                .WithArguments(new PlayerModel(), playerView);
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
                .AsCached();
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
                .WithArguments(enemyPoolParent, levelInfoHolder.EnemyPoints);
        }
        private void BindSupplyPool()
        {
            Container
                .Bind<SupplyFactory>()
                .AsSingle()
                .WithArguments(supplyPrefab);

            Container
                .Bind<SupplyPool>()
                .AsSingle()
                .WithArguments(supplyPoolParent, levelInfoHolder.SupplyPoints);
        }

        private void BindHud()
        {
            Container
                .Bind<HudPresenter>()
                .AsSingle()
                .WithArguments(new HudModel(), hudView);
        }
    }
}
