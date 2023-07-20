using System;
using Base;
using Scriptables;
using UniRx;
using UnityEngine;
using Zenject;

namespace Infrastructure.Services.Input
{
    public class InputService : BaseService
    {
        private readonly InputServiceModel _model;
        
        private readonly PlayerControls _playerControls;
        private readonly ScriptableProjectSettings _projectSettings;
        
        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        private IDisposable _movementSubscription;
        private IDisposable _shootSubscription;

        public Action<Vector2> OnMovementInputChanged;
        public Action<Vector2> OnShootInputChanged;

        
        [Inject]
        public InputService(ScriptableProjectSettings projectSettings)
        {
            _projectSettings = projectSettings;
            
            _playerControls = new PlayerControls();
            _model = new InputServiceModel();
            
            Init();
        }

        private void Init()
        {
            _movementSubscription = _model.Observe()
                .Select(model => model.MovementInput)
                .DistinctUntilChanged(state => state.GetHashCode())
                .Subscribe(
                    value =>
                    {
                        _model.Update();
                        OnMovementInputChanged?.Invoke(value);
                    })
                .AddTo(_disposable);
            
            _shootSubscription = _model.Observe()
                .Select(model => model.ShootInput)
                .DistinctUntilChanged(state => state.GetHashCode())
                .Subscribe(
                    value =>
                    {
                        if(value > 0)
                        {
                            OnShootInputChanged?.Invoke(_model.MouseInput);
                        }
                    })
                .AddTo(_disposable);
        }

        public override void Update()
        {
            _model.MovementInput = _playerControls.Player.Movement.ReadValue<Vector2>();
            _model.MouseInput = _playerControls.Player.Mouse.ReadValue<Vector2>();
            _model.ShootInput = _playerControls.Player.Shoot.ReadValue<float>();
            _model.Update();
        }

        public override void OnEnable()
        {
            _playerControls.Enable();
        }
        
        public override void OnDisable()
        {
            _playerControls.Disable();
        }
    }
}
