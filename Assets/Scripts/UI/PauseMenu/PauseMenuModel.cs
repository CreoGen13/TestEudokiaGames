using System;
using Base.Classes;
using UniRx;

namespace UI.PauseMenu
{
    public class PauseMenuModel : BaseModel
    {
        private readonly BehaviorSubject<PauseMenuModel> _subject;

        public bool IsValidating;

        public PauseMenuModel()
        {
            _subject = new BehaviorSubject<PauseMenuModel>(this);
        }

        public IObservable<PauseMenuModel> Observe()
        {
            return _subject.AsObservable();
        }

        public void Update()
        {
            _subject.OnNext(this);
        }
    }
}