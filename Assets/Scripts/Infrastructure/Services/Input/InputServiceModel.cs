using System;
using UniRx;
using Vector2 = UnityEngine.Vector2;

namespace Infrastructure.Services.Input
{
    public class InputServiceModel
    {
        private readonly BehaviorSubject<InputServiceModel> _subject;

        public Vector2 MovementInput;
        public Vector2 MouseInput;
        public float ShootInput;
        
        public InputServiceModel()
        {
            _subject = new BehaviorSubject<InputServiceModel>(this);
        }

        public IObservable<InputServiceModel> Observe()
        {
            return _subject.AsObservable();
        }

        public void Update()
        {
            _subject.OnNext(this);
        }
    }
}