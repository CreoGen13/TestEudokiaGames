using System;
using Base.Classes;
using UniRx;

namespace Game
{
    public class GameModel : BaseModel
    {
        private readonly BehaviorSubject<GameModel> _subject;

        public EnemyCoefficients EnemyCoefficients;
        public PlayerCoefficients PlayerCoefficients;
        public ProjectileCoefficients ProjectileCoefficients;
        
        public float SpawnTime;
        public int Points;
        
        public GameModel()
        {
            _subject = new BehaviorSubject<GameModel>(this);
        }

        public IObservable<GameModel> Observe()
        {
            return _subject.AsObservable();
        }

        public void Update()
        {
            _subject.OnNext(this);
        }
    }
}