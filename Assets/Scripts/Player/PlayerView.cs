using System;
using Base;
using Base.Classes;
using Base.Interfaces;
using DG.Tweening;
using Infrastructure.Pools.Projectile;
using Infrastructure.Services.Input;
using Projectile;
using Scriptables;
using UnityEngine;
using Zenject;

namespace Player
{
    public class PlayerView : BaseView, IProceduralView
    {
        [SerializeField] private CharacterController characterController;
        [SerializeField] private Transform cameraFollow;
        [SerializeField] private Transform projectileSpawnPoint;

        private ScriptablePlayerSettings _playerSettings;
        private InputService _inputService;
        private Camera _playerCamera;
        private PlayerPresenter _presenter;

        private bool _isRotating;
        private Sequence _rotationSequence;

        public Transform CameraFollow => cameraFollow;

        [Inject]
        private void Construct(ScriptablePlayerSettings playerSettings, Camera playerCamera)
        {
            _playerSettings = playerSettings;
            _playerCamera = playerCamera;
        }

        public void SetPresenter(BasePresenter presenter)
        {
            _presenter = (PlayerPresenter)presenter;
        }

        private void Start()
        {
            _presenter.Start();
        }

        public void Move(Vector2 inputAxis)
        {
            if(_isRotating || inputAxis == Vector2.zero ||  inputAxis == Vector2.up)
                return;
            
            _isRotating = true;
            var direction = new Vector3(inputAxis.x, 0, inputAxis.y);
            var angle = Vector3.SignedAngle(Vector3.forward, direction, Vector3.up);
            var duration = Mathf.Abs(angle) / _playerSettings.playerRotationSpeed;
            var rotateAngle = transform.rotation.eulerAngles + new Vector3(0, angle, 0);

            _rotationSequence = DOTween.Sequence();
            _rotationSequence.Join(transform.DORotate(rotateAngle, duration));
            _rotationSequence.OnComplete(() =>
            {
                _isRotating = false;
            });
        }

        public void Shoot(Vector2 mousePosition, ProjectilePresenter projectile)
        {
            projectile.SetPosition(projectileSpawnPoint.position);
            
            Ray ray = _playerCamera.ScreenPointToRay(mousePosition);
            if (Physics.Raycast(ray, out var hit, _playerSettings.projectileForwardForce))
            {
                var forwardDirection = (hit.point - projectileSpawnPoint.position).normalized;
                var upDirection = Quaternion.LookRotation(forwardDirection) * Vector3.up;
                var force = forwardDirection * _playerSettings.projectileForwardForce +
                            upDirection * _playerSettings.projectileUpForce;
                projectile.AddForce(force, ForceMode.Force);
            }
            else
            {
                var upDirection = Quaternion.LookRotation(ray.direction) * Vector3.up;
                var force = ray.direction * _playerSettings.projectileForwardForce +
                            upDirection * _playerSettings.projectileUpForce;
                projectile.AddForce(force, ForceMode.Force);
            }
        }

        private void OnDestroy()
        {
            _presenter.Dispose();
        }
    }
}