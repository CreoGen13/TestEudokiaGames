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
        private readonly TimeService _timeService;
        private readonly ScriptablePlayerSettings _playerSettings;
        private readonly IProjectilePool _projectilePool;

        private Timer _reloadTimer;

        public PlayerPresenter(PlayerModel model, PlayerView view, ScriptablePlayerSettings playerSettings, IProjectilePool projectilePool, InputService inputService, TimeService timeService)
            : base(model, view)
        {
            _playerSettings = playerSettings;
            _projectilePool = projectilePool;
            _timeService = timeService;
            
            inputService.OnMovementInputChanged += View.Move;
            inputService.OnShootInputChanged += Shoot;
            
            Reset();
        }

        protected override void Init()
        {
            throw new System.NotImplementedException();
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
    }
}