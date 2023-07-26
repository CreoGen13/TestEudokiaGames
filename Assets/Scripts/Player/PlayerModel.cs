using System;
using Base;
using Base.Classes;
using UniRx;

namespace Player
{
    public class PlayerModel : BaseModel
    {
        private readonly BehaviorSubject<PlayerModel> _subject;

        public bool IsReloaded;
        public float ReloadTime;

        public PlayerModel()
        {
            _subject = new BehaviorSubject<PlayerModel>(this);
        }

        public IObservable<PlayerModel> Observe()
        {
            return _subject.AsObservable();
        }

        public void Update()
        {
            _subject.OnNext(this);
        }
    }
}