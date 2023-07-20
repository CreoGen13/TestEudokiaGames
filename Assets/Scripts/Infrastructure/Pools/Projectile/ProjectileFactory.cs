using Base;
using UnityEngine;
using Zenject;

namespace Infrastructure.Pools.Projectile
{
    public class ProjectileFactory : IBaseFactory<Mono.Projectile>
    {
        private readonly DiContainer _container;
        private readonly GameObject _projectilePrefab;

        public ProjectileFactory(GameObject projectilePrefab, DiContainer container)
        {
            _container = container;
            _projectilePrefab = projectilePrefab;
        }

        public Mono.Projectile Create(Transform parent)
        {
            var projectile = _container.InstantiatePrefabForComponent<Mono.Projectile>(_projectilePrefab);
            projectile.transform.SetParent(parent);
            return projectile;
        }
    }
}