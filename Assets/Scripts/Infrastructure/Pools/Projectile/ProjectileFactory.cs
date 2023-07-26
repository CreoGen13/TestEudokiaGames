using Base;
using Base.Interfaces;
using Projectile;
using UnityEngine;
using Zenject;

namespace Infrastructure.Pools.Projectile
{
    public class ProjectileFactory : IBaseFactory<ProjectilePresenter>
    {
        private readonly DiContainer _container;
        private readonly GameObject _projectilePrefab;
        
        private int _count;

        public ProjectileFactory(GameObject projectilePrefab, DiContainer container)
        {
            _container = container;
            _projectilePrefab = projectilePrefab;
        }

        public ProjectilePresenter Create(Transform parent)
        {
            _count++;
            var projectile = _container.InstantiatePrefabForComponent<ProjectileView>(_projectilePrefab, parent);
            
            _container
                .Bind<ProjectilePresenter>()
                .WithId(_count)
                .AsTransient()
                .WithArguments(new ProjectileModel(), projectile)
                .OnInstantiated((context, instance) =>
                {
                    projectile.SetPresenter((ProjectilePresenter)instance);
                })
                .NonLazy();

            return _container.ResolveId<ProjectilePresenter>(_count);
        }
    }
}