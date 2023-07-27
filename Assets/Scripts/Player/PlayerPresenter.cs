using Base.Classes;
using Game;
using Infrastructure.Pools.Projectile;
using Infrastructure.Services.Input;
using Infrastructure.Services.Timer;
using Scriptables;
using UnityEngine;

namespace Player
{
    public class PlayerPresenter : BaseGenericPresenter<PlayerModel, PlayerView>
    {
        private readonly InputService _inputService;
        private readonly TimeService _timeService;
        private readonly ScriptablePlayerSettings _playerSettings;
        private readonly IProjectilePool _projectilePool;

        private Timer _reloadTimer;

        public PlayerPresenter(PlayerModel model, PlayerView view, ScriptablePlayerSettings playerSettings, IProjectilePool projectilePool, InputService inputService, TimeService timeService)
            : base(model, view)
        {
            _inputService = inputService;
            _playerSettings = playerSettings;
            _projectilePool = projectilePool;
            _timeService = timeService;
        }

        public sealed override void Reset()
        {
            _reloadTimer = _timeService.GetTimer(_playerSettings.cannonReloadTime, () =>
            {
                Model.IsReloaded = true;
                Model.Update();
            });
            
            Model.IsReloaded = true;
            Model.ReloadTime = _playerSettings.cannonReloadTime;
            Model.Update();
        }

        protected override void Init()
        {
            _inputService.OnMovementInputChanged += View.Move;
            _inputService.OnShootInputChanged += Shoot;
        }

        public void Start()
        {
            Reset();
            Init();
        }

        private void Shoot(Vector2 mousePosition)
        {
            if(!Model.IsReloaded)
                return;
            
            _timeService.ResumeTimer(_reloadTimer, Model.ReloadTime);
            Model.IsReloaded = false;
            Model.Update();

            var projectile = _projectilePool.Spawn();
            View.Shoot(mousePosition, projectile);
        }

        public void SetCoefficient(float reload)
        {
            Model.ReloadTime = reload;
        }

        public override void Dispose()
        {
            _inputService.OnMovementInputChanged -= View.Move;
            _inputService.OnShootInputChanged -= Shoot;
            
            base.Dispose();
        }
    }
}