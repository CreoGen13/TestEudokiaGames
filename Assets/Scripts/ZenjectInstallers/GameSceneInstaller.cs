using Cinemachine;
using Game;
using Infrastructure.Pools.Enemy;
using Infrastructure.Pools.Projectile;
using Infrastructure.Pools.Supply;
using Mono;
using Player;
using Scriptables;
using UI;
using UI.Hud;
using UI.PauseMenu;
using UnityEngine;
using Zenject;

namespace ZenjectInstallers
{
    public class GameSceneInstaller : MonoInstaller
    {
        [Header("References")]
        [SerializeField] private CinemachineVirtualCamera virtualCamera;
        [SerializeField] private Camera playerCamera;
        [SerializeField] private Transform projectilePoolParent;
        [SerializeField] private Transform enemyPoolParent;
        [SerializeField] private Transform supplyPoolParent;
        [SerializeField] private LevelInfoHolder levelInfoHolder;
        [SerializeField] private GameView gameView;
        [SerializeField] private GameOverScreen gameOverScreen;
        
        [Header("UI")]
        [SerializeField] private HudView hudView;
        [SerializeField] private PauseMenuView pauseMenuView;

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
            BindScriptables();
            BindLevelInfoHolder();
            BindPauseMenu();
            BindHud();
            BindGameOverScreen();

            BindEnemyPool();
            BindSupplyPool();
            BindProjectilePool();

            BindCamera();
            BindPlayer();

            BindGame();
        }

        private void BindGame()
        {
            Container
                .Bind<GamePresenter>()
                .AsSingle()
                .WithArguments(new GameModel(), gameView);
        }
        private void BindLevelInfoHolder()
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
            var playerView = Container.InstantiatePrefabForComponent<PlayerView>(playerPrefab);
            virtualCamera.Follow = playerView.CameraFollow;
            
            Container
                .Bind<PlayerPresenter>()
                .AsSingle()
                .WithArguments(new PlayerModel(), playerView)
                .OnInstantiated((context, instance) =>
                {
                    playerView.SetPresenter((PlayerPresenter)instance);
                })
                .NonLazy();
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
                .BindInterfacesAndSelfTo<ProjectilePool>()
                .AsSingle()
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
        private void BindPauseMenu()
        {
            Container
                .Bind<PauseMenuPresenter>()
                .AsSingle()
                .WithArguments(new PauseMenuModel(), pauseMenuView);
        }

        private void BindGameOverScreen()
        {
            Container
                .Bind<GameOverScreen>()
                .FromInstance(gameOverScreen)
                .AsSingle();
        }
    }
}
