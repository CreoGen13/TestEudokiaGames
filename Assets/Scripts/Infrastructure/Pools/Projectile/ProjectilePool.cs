using System;
using System.Collections.Generic;
using Mono;
using UnityEngine;
using Zenject;

namespace Infrastructure.Pools.Projectile
{
    public class ProjectilePool : IProjectilePool
    {
        public Action<UnitEnemy, Mono.Projectile> OnEnemyHit;
        
        private readonly Transform _parent;
        private readonly ProjectileFactory _factory;
        private readonly Queue<Mono.Projectile> _projectiles = new Queue<Mono.Projectile>();

        [Inject]
        public ProjectilePool(ProjectileFactory factory, Transform parent)
        {
            _parent = parent;
            _factory = factory;
        }

        public void Init(int count)
        {
            for (int i = 0; i < count; i++)
            {
                var projectile = _factory.Create(_parent);
                projectile.gameObject.SetActive(false);
                projectile.OnEnemyHit += (enemy) =>
                {
                    OnEnemyHit?.Invoke(enemy, projectile);
                };
                projectile.OnReturn += () =>
                {
                    Return(projectile);
                };
                
                _projectiles.Enqueue(projectile);
            }
        }

        public Mono.Projectile Spawn(Vector3 position)
        {
            var projectile = _projectiles.Dequeue();
            projectile.gameObject.SetActive(true);
            projectile.transform.position = position;
            return projectile;
        }

        public void Return(Mono.Projectile projectile)
        {
            projectile.gameObject.SetActive(false);
            projectile.Reset();
            _projectiles.Enqueue(projectile);
        }
    }
}