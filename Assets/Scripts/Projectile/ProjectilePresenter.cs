using System;
using Base;
using Base.Classes;
using Game;
using Infrastructure.Services;
using Infrastructure.Services.Timer;
using Scriptables;
using UnityEngine;
using Zenject;

namespace Projectile
{
    public class ProjectilePresenter : BaseGenericPresenter<ProjectileModel, ProjectileView>
    {
        public Action<Collision> OnHit;
        public Action OnReturn;
        
        private readonly ScriptableGameSettings _gameSettings;
        private readonly TimeService _timeService;

        private Timer _lifeTimeTimer;
        private Timer _afterHitLifeTimeTimer;

        [Inject]
        public ProjectilePresenter(ProjectileModel model, ProjectileView view, ScriptableGameSettings gameSettings, TimeService timeService)
            : base(model, view)
        {
            _timeService = timeService;
            _gameSettings = gameSettings;
            
            Reset();
            Init();
        }
        protected sealed override void Init()
        {
            
        }

        public void SetActive(bool active)
        {
            View.gameObject.SetActive(active);
        }
        public void SetPosition(Vector3 position)
        {
            View.gameObject.transform.position = position;
        }
        public void SetCoefficient(float damage)
        {
            Model.Damage = damage;
            Model.Update();
        }

        public float GetDamage()
        {
            return Model.Damage;
        }
        public void AddForce(Vector3 force, ForceMode forceMode)
        {
            View.Rigidbody.AddForce(force, forceMode);
            
            _lifeTimeTimer = _timeService.GetTimer(_gameSettings.projectileLifeTime, () =>
            {
                OnReturn?.Invoke();
            });
        }
        public void Hit(Collision collision)
        {
            if(!Model.IsActive)
                return;
            
            Model.IsActive = false;
            Model.Update();
            
            OnHit?.Invoke(collision);
            
            _afterHitLifeTimeTimer = _timeService.GetTimer(_gameSettings.projectileAfterHitLifeTime, () =>
            {
                OnReturn?.Invoke();
            });
        }

        public sealed override void Reset()
        {
            View.Rigidbody.velocity = Vector3.zero;
            
            _lifeTimeTimer?.Destroy();
            _afterHitLifeTimeTimer?.Destroy();

            Model.IsActive = true;
            Model.Update();
        }
    }
}