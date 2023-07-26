using System;
using Base;
using Base.Classes;
using Game;
using Scriptables;
using UniRx;
using UnityEngine;
using Zenject;

namespace Enemy
{
    public class EnemyPresenter : BaseGenericPresenter<EnemyModel, EnemyView>
    {
        public Action OnDestinationReached;
        public Action OnDeath;
        
        private IDisposable _destinationSubscription;
        private IDisposable _healthSubscription;
        private IDisposable _speedSubscription;

        private const float Tolerance = 0.01f;
        
        [Inject]
        public EnemyPresenter(EnemyModel model, EnemyView view)
            : base(model, view)
        {
            Reset();
            Init();
        }

        protected sealed override void Init()
        {
            _destinationSubscription = Model.Observe()
                .Select(model => model.RemainingDistance)
                .DistinctUntilChanged(state => state.GetHashCode())
                .Subscribe(
                    value =>
                    {
                        if (value < Tolerance)
                        {
                            OnDestinationReached?.Invoke();
                        }
                    })
                .AddTo(Disposable);
            
            _healthSubscription = Model.Observe()
                .Select(model => model.Health)
                .DistinctUntilChanged(state => state.GetHashCode())
                .Subscribe(
                    value =>
                    {
                        View.SetHealth(value / Model.MaxHealth);
                        if (value == 0)
                        {
                            OnDeath?.Invoke();
                        }
                    })
                .AddTo(Disposable);
            
            _speedSubscription = Model.Observe()
                .Select(model => model.Speed)
                .DistinctUntilChanged(state => state.GetHashCode())
                .Subscribe(
                    value =>
                    {
                        View.NavMeshAgent.speed = value;
                    })
                .AddTo(Disposable);
        }

        public void Update()
        {
            Model.RemainingDistance = View.NavMeshAgent.remainingDistance;
            Model.Update();
        }

        public void ReceiveDamage(float damage)
        {
            Model.Health = Mathf.Clamp(Model.Health - damage, 0, float.MaxValue);
            Model.Update();
        }
        
        public void SetActive(bool active)
        {
            View.gameObject.SetActive(active);
        }
        public void SetPosition(Vector3 position)
        {
            View.gameObject.transform.position = position;
        }
        public void SetDestination(Vector3 destination)
        {
            View.NavMeshAgent.destination = destination;
        }

        public void SetStats(float health, float speed)
        {
            Model.MaxHealth = health;
            Model.Health = Model.MaxHealth;
            Model.Speed = speed;
            Model.Update();
        }
        public sealed override void Reset()
        {
            Model.Health = Model.MaxHealth;
            Model.Update();
        }
    }
}