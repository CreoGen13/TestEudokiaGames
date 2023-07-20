using DG.Tweening;
using Infrastructure.Pools.Projectile;
using Infrastructure.Services.Input;
using Scriptables;
using UnityEngine;
using Zenject;

namespace Mono
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private CharacterController characterController;
        [SerializeField] private Transform cameraFollow;
        [SerializeField] private Transform projectileSpawnPoint;

        private ScriptablePlayerSettings _playerSettings;
        private InputService _inputService;
        private IProjectilePool _projectilePool;
        private Camera _playerCamera;

        private bool _isRotating;
        private Sequence _rotationSequence;

        public Transform CameraFollow => cameraFollow;
        
        [Inject]
        private void Construct(InputService inputService, ScriptablePlayerSettings playerSettings, IProjectilePool projectilePool, Camera playerCamera)
        {
            _playerSettings = playerSettings;
            _inputService = inputService;
            _projectilePool = projectilePool;
            _playerCamera = playerCamera;
        }

        private void Start()
        {
            _inputService.OnMovementInputChanged += Move;
            _inputService.OnShootInputChanged += Shoot;
        }

        private void Move(Vector2 inputAxis)
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
        private void Shoot(Vector2 mousePosition)
        {
            var projectile = _projectilePool.Spawn(projectileSpawnPoint.position);

            Ray ray = _playerCamera.ScreenPointToRay(mousePosition);
            if (Physics.Raycast(ray, out var hit, 100))
            {
                var forwardDirection = (hit.point - projectileSpawnPoint.position).normalized;
                var upDirection = Quaternion.LookRotation(forwardDirection) * Vector3.up;
                var force = forwardDirection * _playerSettings.projectileForwardForce +
                            upDirection * _playerSettings.projectileUpForce;
                projectile.Rigidbody.AddForce(force, ForceMode.Force);
            }
            else
            {
                var upDirection = Quaternion.LookRotation(ray.direction) * Vector3.up;
                var force = ray.direction * _playerSettings.projectileForwardForce +
                            upDirection * _playerSettings.projectileUpForce;
                projectile.Rigidbody.AddForce(force, ForceMode.Force);
            }
        }
    }
}