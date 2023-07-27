using System;
using Base.Classes;
using UniRx;

namespace UI.MainMenu
{
    public class MainMenuModel : BaseModel
    {
        private readonly BehaviorSubject<MainMenuModel> _subject;

        public bool IsValidating;
        
        public MainMenuModel()
        {
            _subject = new BehaviorSubject<MainMenuModel>(this);
        }

        public IObservable<MainMenuModel> Observe()
        {
            return _subject.AsObservable();
        }

        public void Update()
        {
            _subject.OnNext(this);
        }
    }
}