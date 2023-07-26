using System;
using Base;
using Base.Classes;
using UniRx;

namespace Projectile
{
    public class ProjectileModel : BaseModel
    {
        private readonly BehaviorSubject<ProjectileModel> _subject;
        
        public bool IsActive;
        public float Damage;
        
        public ProjectileModel()
        {
            _subject = new BehaviorSubject<ProjectileModel>(this);
        }

        public IObservable<ProjectileModel> Observe()
        {
            return _subject.AsObservable();
        }

        public void Update()
        {
            _subject.OnNext(this);
        }
    }
}