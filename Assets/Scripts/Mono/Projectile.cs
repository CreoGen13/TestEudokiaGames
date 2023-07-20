using System;
using DG.Tweening;
using ModestTree;
using Scriptables;
using UnityEngine;
using Zenject;

namespace Mono
{
    public class Projectile : MonoBehaviour
    {
        public Action<UnitEnemy> OnEnemyHit;
        public Action OnReturn;
        
        [SerializeField] private Rigidbody rigidBody;
        public Rigidbody Rigidbody => rigidBody;
        
        private string _enemyTag;
        private float _lifeTime;
        private bool _isActive = true;
        
        [Inject]
        private void Construct(ScriptableGameSettings gameSettings)
        {
            _enemyTag = gameSettings.enemyTag;
            _lifeTime = gameSettings.projectileLifeTime;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if(!_isActive)
                return;

            if (!collision.gameObject.tag.IsEmpty() && collision.gameObject.CompareTag(_enemyTag))
            {
                OnEnemyHit?.Invoke(collision.gameObject.GetComponent<UnitEnemy>());
            }
            else
            {
                int x = 0;
                DOTween.To(() => x, y => x = y, 1, _lifeTime).OnComplete(() =>
                {
                    OnReturn?.Invoke();
                });
            }
            _isActive = false;
        }

        public void Reset()
        {
            _isActive = true;
        }
    }
}