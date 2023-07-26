using System;
using System.Collections.Generic;
using Enemy;
using Game;
using Mono;
using Projectile;
using UnityEngine;
using Zenject;

namespace Infrastructure.Pools.Projectile
{
    public class ProjectilePool : IProjectilePool
    {
        public Action<Collision, ProjectilePresenter> OnHit;
        
        private readonly Transform _parent;
        private readonly ProjectileFactory _factory;
        private readonly Queue<ProjectilePresenter> _projectiles = new Queue<ProjectilePresenter>();
        private readonly List<ProjectilePresenter> _spawnedProjectiles = new List<ProjectilePresenter>();
        
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
                projectile.SetActive(false);
                projectile.OnHit += (collision) =>
                {
                    OnHit?.Invoke(collision, projectile);
                };
                projectile.OnReturn += () =>
                {
                    Return(projectile);
                };
                
                _projectiles.Enqueue(projectile);
            }
        }

        public void Clear()
        {
            foreach (var spawnedProjectile in _spawnedProjectiles)
            {
                Return(spawnedProjectile);
            }
        }
        public ProjectilePresenter Spawn()
        {
            var projectile = _projectiles.Dequeue();
            projectile.SetActive(true);
            _spawnedProjectiles.Add(projectile);
            
            return projectile;
        }
        public ProjectilePresenter Spawn(Vector3 position)
        {
            var projectile = _projectiles.Dequeue();
            projectile.SetActive(true);
            projectile.SetPosition(position);
            _spawnedProjectiles.Add(projectile);
            
            return projectile;
        }
        public void Return(ProjectilePresenter projectile)
        {
            if(_projectiles.Contains(projectile))
                return;
            
            projectile.SetActive(false);
            projectile.Reset();
            _projectiles.Enqueue(projectile);
            _spawnedProjectiles.Remove(projectile);
        }
        
        public void SetCoefficient(float damage)
        {
            foreach (var projectile in _projectiles)
            {
                projectile.SetCoefficient(damage);
            }
        }
    }
}