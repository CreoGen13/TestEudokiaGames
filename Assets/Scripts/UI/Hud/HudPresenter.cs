using System;
using Base.Classes;
using UniRx;
using Zenject;

namespace UI.Hud
{
    public class HudPresenter : BaseGenericPresenter<HudModel, HudView>
    {
        private IDisposable _damageSubscription;
        private IDisposable _reloadTimeSubscription;
        private IDisposable _enemyHealthSubscription;
        private IDisposable _enemySpeedSubscription;
        private IDisposable _enemySpawnTimeSubscription;
        private IDisposable _supplySpawnSubscription;
        private IDisposable _upgradeSubscription;
        private IDisposable _enemiesLeftSubscription;
        private IDisposable _pointsSubscription;

        private const string Format = "#0.00";
        
        [Inject]
        public HudPresenter(HudModel model, HudView view)
            : base(model, view)
        {
            Reset();
            Init();
        }

        protected sealed override void Init()
        {
            _damageSubscription = Model.Observe()
                .Select(model => model.Damage)
                .DistinctUntilChanged(state => state.GetHashCode())
                .Subscribe(
                    value =>
                    {
                        View.damageStat.text = value.ToString(Format);
                    })
                .AddTo(Disposable);
            
            _reloadTimeSubscription = Model.Observe()
                .Select(model => model.ReloadTime)
                .DistinctUntilChanged(state => state.GetHashCode())
                .Subscribe(
                    value =>
                    {
                        View.reloadStat.text = value.ToString(Format);
                    })
                .AddTo(Disposable);
            
            _enemyHealthSubscription = Model.Observe()
                .Select(model => model.EnemyHealth)
                .DistinctUntilChanged(state => state.GetHashCode())
                .Subscribe(
                    value =>
                    {
                        View.enemyHealthStat.text = value.ToString(Format);
                    })
                .AddTo(Disposable);
            
            _enemySpeedSubscription = Model.Observe()
                .Select(model => model.EnemySpeed)
                .DistinctUntilChanged(state => state.GetHashCode())
                .Subscribe(
                    value =>
                    {
                        View.enemySpeedStat.text = value.ToString(Format);
                    })
                .AddTo(Disposable);
            
            _enemySpawnTimeSubscription = Model.Observe()
                .Select(model => model.EnemySpawnTime)
                .DistinctUntilChanged(state => state.GetHashCode())
                .Subscribe(
                    value =>
                    {
                        View.enemySpawnStat.text = value.ToString(Format);
                    })
                .AddTo(Disposable);
            
            _supplySpawnSubscription = Model.Observe()
                .Select(model => model.SupplySpawnTime)
                .DistinctUntilChanged(state => state.GetHashCode())
                .Subscribe(
                    value =>
                    {
                        View.supplySpawnStat.text = value.ToString(Format);
                    })
                .AddTo(Disposable);
            
            _upgradeSubscription = Model.Observe()
                .Select(model => model.UpgradeTime)
                .DistinctUntilChanged(state => state.GetHashCode())
                .Subscribe(
                    value =>
                    {
                        View.upgradeStat.text = value.ToString(Format);
                    })
                .AddTo(Disposable);
            
            _enemiesLeftSubscription = Model.Observe()
                .Select(model => model.EnemiesLeft)
                .DistinctUntilChanged(state => state.GetHashCode())
                .Subscribe(
                    value =>
                    {
                        View.enemiesLeft.text = value.ToString();
                    })
                .AddTo(Disposable);
            
            _pointsSubscription = Model.Observe()
                .Select(model => model.Points)
                .DistinctUntilChanged(state => state.GetHashCode())
                .Subscribe(
                    value =>
                    {
                        View.points.text = value.ToString();
                    })
                .AddTo(Disposable);
        }

        public void SetUpgradeTime(float time)
        {
            Model.UpgradeTime = time;
            Model.Update();
        }
        public void SetEnemySpawnTime(float time)
        {
            Model.EnemySpawnTime = time;
            Model.Update();
        }
        public void SetSupplySpawnTime(float time)
        {
            Model.SupplySpawnTime = time;
            Model.Update();
        }

        public void SetEnemyStats(float health, float speed)
        {
            Model.EnemyHealth = health;
            Model.EnemySpeed = speed;
            Model.Update();
        }
        public void SetPlayerDamage(float damage)
        {
            Model.Damage = damage;
            Model.Update();
        }
        public void SetPlayerReload(float reload)
        {
            Model.ReloadTime = reload;
            Model.Update();
        }
        public void SetEnemiesLeft(int enemiesLeft)
        {
            Model.EnemiesLeft = enemiesLeft;
            Model.Update();
        }
        public void SetPoints(int points)
        {
            Model.Points = points;
            Model.Update();
        }
        
        public sealed override void Reset()
        {
            Model.Damage = 0;
            Model.ReloadTime = 0;
            Model.EnemyHealth = 0;
            Model.EnemySpeed = 0;
            Model.EnemySpawnTime = 0;
            Model.SupplySpawnTime = 0;
            Model.UpgradeTime = 0;
            Model.EnemiesLeft = 0;
            Model.Points = 0;
            Model.Update();
        }
    }
}