using System;
using Base;
using Base.Classes;
using Zenject;

namespace Game
{
    public class GameView : BaseView
    {
        private GamePresenter _presenter;
        
        [Inject]
        private void Construct(GamePresenter gamePresenter)
        {
            _presenter = gamePresenter;
        }

        private void Start()
        {
            _presenter.Start();
        }

        private void Update()
        {
            foreach (var service in _presenter.Services)
            {
                service.Update();
            }
        }
        private void OnEnable()
        {
            foreach (var service in _presenter.Services)
            {
                service.OnEnable();
            }
        }
        private void OnDisable()
        {
            _presenter.Dispose();
            foreach (var service in _presenter.Services)
            {
                service.OnDisable();
            }
        }
    }
}