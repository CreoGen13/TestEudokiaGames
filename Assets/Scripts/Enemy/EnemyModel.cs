using System;
using Base;
using Base.Classes;
using UniRx;
using UnityEngine;
using Zenject;

namespace Enemy
{
    public class EnemyModel : BaseModel
    {
        private readonly BehaviorSubject<EnemyModel> _subject;

        public float Speed;
        public float Health;
        public float MaxHealth;
        public float RemainingDistance;
        
        public EnemyModel()
        {
            _subject = new BehaviorSubject<EnemyModel>(this);
        }

        public IObservable<EnemyModel> Observe()
        {
            return _subject.AsObservable();
        }

        public void Update()
        {
            _subject.OnNext(this);
        }
    }
}