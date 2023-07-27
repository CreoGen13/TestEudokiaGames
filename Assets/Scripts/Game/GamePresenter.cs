using System;
using System.Collections.Generic;
using Base.Classes;
using Enemy;
using Infrastructure.Pools.Enemy;
using Infrastructure.Pools.Projectile;
using Infrastructure.Pools.Supply;
using Infrastructure.Services.Input;
using Infrastructure.Services.Timer;
using Infrastructure.Utils;
using Mono;
using Player;
using Projectile;
using Scriptables;
using UI;
using UI.Hud;
using UI.PauseMenu;
using UniRx;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Game
{
    public class GamePresenter : BaseGenericPresenter<GameModel, GameView>
    {
        public readonly List<BaseService> Services = new List<BaseService>();
        
        private readonly ScriptableProjectSettings _projectSettings;
        private readonly ScriptableGameSettings _gameSettings;
        private readonly InputService _inputService;
        private readonly TimeService _timeService;
        private readonly LevelInfoHolder _levelInfoHolder;
        private readonly ProjectilePool _projectilePool;
        private readonly EnemyPool _enemyPool;
        private readonly SupplyPool _supplyPool;
        private readonly PlayerPresenter _player;
        private readonly HudPresenter _hudPresenter;
        private readonly PauseMenuPresenter _pauseMenuPresenter;
        private readonly GameOverScreen _gameOverScreen;

        private Timer _upgradeTimer;
        private Timer _spawnEnemyTimer;
        private Timer _spawnSupplyTimer;

        private IDisposable _enemyCoefficientSubscription;
        private IDisposable _playerCoefficientSubscription;
        private IDisposable _projectileCoefficientSubscription;
        private IDisposable _enemiesLeftSubscription;
        private IDisposable _pointsSubscription;
        private IDisposable _pauseSubscription;
        private IDisposable _gameOverSubscription;

        [Inject]
        public GamePresenter(GameModel gameModel, GameView gameView,
            ScriptableProjectSettings projectSettings,
            ScriptableGameSettings gameSettings,
            InputService inputService,
            TimeService timeService,
            LevelInfoHolder levelInfoHolder,
            PlayerPresenter playerPresenter,
            ProjectilePool projectilePool,
            EnemyPool enemyPool,
            SupplyPool supplyPool,
            HudPresenter hudPresenter,
            PauseMenuPresenter pauseMenuPresenter,
            GameOverScreen gameOverScreen)
            : base(gameModel, gameView)
        {
            _gameOverScreen = gameOverScreen;
            _inputService = inputService;
            _pauseMenuPresenter = pauseMenuPresenter;
            _projectSettings = projectSettings;
            _hudPresenter = hudPresenter;
            _timeService = timeService;
            _player = playerPresenter;
            _gameSettings = gameSettings;
            _levelInfoHolder = levelInfoHolder;
            _projectilePool = projectilePool;
            _enemyPool = enemyPool;
            _supplyPool = supplyPool;
            
            Services.Add(inputService);
            Services.Add(timeService);
        }

        public sealed override void Reset()
        {
            _projectilePool.Clear();
            _enemyPool.Clear();
            _supplyPool.Clear();

            _upgradeTimer = _timeService.GetTimer(_gameSettings.upgradeTime,
                () =>
                {
                    var coefficient = _gameSettings.gameCoefficient;
                    Model.EnemyCoefficients = new EnemyCoefficients(Model.EnemyCoefficients.Health * (1 + coefficient), Model.EnemyCoefficients.Speed * (1 + coefficient));
                    Model.SpawnTime *= 1 / (1 + coefficient);
                    Model.Update();

                    _timeService.ResumeTimer(_upgradeTimer, _gameSettings.upgradeTime);
                },
                (time) =>
                {
                    _hudPresenter.SetUpgradeTime(time);
                });
            _spawnEnemyTimer = _timeService.GetTimer(_gameSettings.spawnEnemyTime,
                () =>
                {
                    _enemyPool.SpawnRandom();

                    _timeService.ResumeTimer(_spawnEnemyTimer, Model.SpawnTime);
                },
                (time) =>
                {
                    _hudPresenter.SetEnemySpawnTime(time);
                });
            _spawnSupplyTimer = _timeService.GetTimer(_gameSettings.spawnSupplyTime,
                () =>
                {
                    _supplyPool.SpawnRandom();

                    _timeService.ResumeTimer(_spawnSupplyTimer, _gameSettings.spawnSupplyTime);
                },
                (time) =>
                {
                    _hudPresenter.SetSupplySpawnTime(time);
                });
            
            Model.EnemyCoefficients = new EnemyCoefficients(1, 1);
            Model.PlayerCoefficients = new PlayerCoefficients(1);
            Model.ProjectileCoefficients = new ProjectileCoefficients(1);
            Model.SpawnTime = _gameSettings.spawnEnemyTime;
            Model.IsPaused = false;
            Model.IsGameOver = false;
            Model.Update();
        }
        protected sealed override void Init()
        {
            _inputService.OnMenuInputChanged += OpenPauseMenu;
            _inputService.OnUpgradeDamageInputChanged += UpgradeDamage;
            _inputService.OnUpgradeReloadInputChanged += UpgradeReload;
            
            _pauseMenuPresenter.OnMenuClosed += ResumeGame;
            
            _projectilePool.Init(_projectSettings.poolSize);
            _projectilePool.OnHit += HitEntity;
            
            _enemyPool.Init(_projectSettings.poolSize);
            _enemyPool.OnDestinationReached += ChangeEnemyDestination;
            _enemyPool.OnDeath += AddPoint;
            _enemyPool.OnCountChanged += (count) =>
            {
                Model.EnemiesLeft = count;
                Model.Update();
            };
            
            _supplyPool.Init(_projectSettings.poolSize);
            
            _enemyCoefficientSubscription = Model.Observe()
                .Select(model => model.EnemyCoefficients)
                .DistinctUntilChanged(state => state.GetHashCode())
                .Subscribe(
                    value =>
                    {
                        var health = value.Health * _gameSettings.enemyHealth;
                        var speed = value.Speed * _gameSettings.enemySpeed;
                        
                        _enemyPool.SetStats(health, speed);
                        _hudPresenter.SetEnemyStats(health, speed);
                    })
                .AddTo(Disposable);
            
            _projectileCoefficientSubscription = Model.Observe()
                .Select(model => model.ProjectileCoefficients)
                .DistinctUntilChanged(state => state.GetHashCode())
                .Subscribe(
                    value =>
                    {
                        var damage = value.Damage * _gameSettings.projectileDamage;
                        _projectilePool.SetCoefficient(damage);
                        _hudPresenter.SetPlayerDamage(damage);
                    })
                .AddTo(Disposable);
            
            _playerCoefficientSubscription = Model.Observe()
                .Select(model => model.PlayerCoefficients)
                .DistinctUntilChanged(state => state.GetHashCode())
                .Subscribe(
                    value =>
                    {
                        var reload = value.Reload;
                        _player.SetCoefficient(reload);
                        _hudPresenter.SetPlayerReload(reload);
                    })
                .AddTo(Disposable);
            
            _pointsSubscription = Model.Observe()
                .Select(model => model.Points)
                .DistinctUntilChanged(state => state.GetHashCode())
                .Subscribe(
                    value =>
                    {
                        _hudPresenter.SetPoints(value);
                    })
                .AddTo(Disposable);
            
            _pauseSubscription = Model.Observe()
                .Select(model => model.IsPaused)
                .DistinctUntilChanged(state => state)
                .Subscribe(
                    value =>
                    {
                        if(Model.IsGameOver)
                            return;
                        
                        Time.timeScale = value ? 0 : 1;
                        
                        foreach (var service in Services)
                        {
                            if(value)
                            {
                                service.Stop();
                            }
                            else
                            {
                                service.Resume();
                            }
                        }
                    })
                .AddTo(Disposable);
            
            _enemiesLeftSubscription = Model.Observe()
                .Select(model => model.EnemiesLeft)
                .DistinctUntilChanged(state => state.GetHashCode())
                .Subscribe(
                    value =>
                    {
                        _hudPresenter.SetEnemiesLeft(value);
                        if (value >= 10)
                        {
                            Model.IsGameOver = true;
                            Model.Update();
                        }
                    })
                .AddTo(Disposable);
            _gameOverSubscription = Model.Observe()
                .Select(model => model.IsGameOver)
                .DistinctUntilChanged(state => state)
                .Subscribe(
                    value =>
                    {
                        if (value)
                        {
                            Time.timeScale = 0;
                            _gameOverScreen.Open();
                            foreach (var service in Services)
                            {
                                service.Stop();
                            }
                        }
                    })
                .AddTo(Disposable);
        }

        public void Start()
        {
            Reset();
            Init();
            
            foreach (var service in Services)
            {
                service.Resume();
            }
        }
        private void PauseGame()
        {
            Model.IsPaused = true;
            Model.Update();
            
        }
        private void ResumeGame()
        {
            Model.IsPaused = false;
            Model.Update();
        }

        private void OpenPauseMenu()
        {
            if(Model.IsPaused)
                return;
            
            PauseGame();
            _pauseMenuPresenter.OpenMenu();
        }
        
        private void UpgradeDamage()
        {
            Model.ProjectileCoefficients =
                new ProjectileCoefficients(Model.ProjectileCoefficients.Damage * (1 + _gameSettings.gameCoefficient));
            Model.Update();
        }

        private void UpgradeReload()
        {
            Model.PlayerCoefficients =
                new PlayerCoefficients(Model.PlayerCoefficients.Reload * (1 /(1 + _gameSettings.gameCoefficient)));
            Model.Update();
        }

        private void AddPoint()
        {
            Model.Points++;
            Model.Update();
        }

        private void HitEntity(Collision collision, ProjectilePresenter projectilePresenter)
        {
            if (collision.gameObject.CompareTag(_gameSettings.enemyTag))
            {
                var enemyPresenter = collision.gameObject.GetComponent<EnemyView>().Presenter;
                enemyPresenter.ReceiveDamage(projectilePresenter.GetDamage());
                _projectilePool.Return(projectilePresenter);
            }
            else if (collision.gameObject.CompareTag(_gameSettings.supplyTag))
            {
                var supply = collision.gameObject.GetComponent<Supply>();
                switch (supply.SupplyType)
                {
                    case SupplyType.KillAll:
                    {
                        Model.Points += _enemyPool.Clear();
                        Model.Update();
                        
                        break;
                    }
                    case SupplyType.StopSpawn:
                    {
                        _timeService.StopTimer(_spawnEnemyTimer, _gameSettings.stopSpawnEnemyTime);
                        
                        break;
                    }
                }
                _supplyPool.Return(supply);
                _projectilePool.Return(projectilePresenter);
            }
        }

        private void ChangeEnemyDestination(EnemyPresenter enemyPresenter)
        {
            var randomDirection = ExtraOperationsUtil.GetRandomPlanePosition(_levelInfoHolder.NavMeshPlane);
            var finalPosition = Vector3.zero;
            if (NavMesh.SamplePosition(randomDirection, out var hit, float.MaxValue, 1)) {
                finalPosition = hit.position;            
            }
            enemyPresenter.SetDestination(finalPosition);
        }

        public override void Dispose()
        {
            _inputService.OnMenuInputChanged -= OpenPauseMenu;
            _inputService.OnUpgradeDamageInputChanged -= UpgradeDamage;
            _inputService.OnUpgradeReloadInputChanged -= UpgradeReload;
            
            base.Dispose();
        }
    }

    public readonly struct EnemyCoefficients
    {
        public readonly float Health;
        public readonly float Speed;

        public EnemyCoefficients(float health, float speed)
        {
            Health = health;
            Speed = speed;
        }

        public override int GetHashCode()
        {
            return (int)(Speed * 100 + Health * 10000);
        }
    }

    public struct PlayerCoefficients
    {
        public readonly float Reload;

        public PlayerCoefficients(float reload)
        {
            Reload = reload;
        }
    }

    public struct ProjectileCoefficients
    {
        public readonly float Damage;

        public ProjectileCoefficients(float damage)
        {
            Damage = damage;
        }
    }
}