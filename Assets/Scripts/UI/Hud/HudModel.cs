using System;
using Base.Classes;
using UniRx;

namespace UI.Hud
{
    public class HudModel : BaseModel
    {
        private readonly BehaviorSubject<HudModel> _subject;

        public float Damage;
        public float ReloadTime;
        public float EnemyHealth;
        public float EnemySpeed;
        
        public float EnemySpawnTime;
        public float SupplySpawnTime;
        public float UpgradeTime;
        
        public int EnemiesLeft;
        public int Points;
        
        public HudModel()
        {
            _subject = new BehaviorSubject<HudModel>(this);
        }

        public IObservable<HudModel> Observe()
        {
            return _subject.AsObservable();
        }

        public void Update()
        {
            _subject.OnNext(this);
        }
    }
}