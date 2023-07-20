using System.Collections.Generic;
using Base;
using Infrastructure.Pools.Enemy;
using Infrastructure.Pools.Projectile;
using Infrastructure.Services.Input;
using Scriptables;
using UnityEngine;
using Zenject;

namespace Mono
{
    public class Game : MonoBehaviour
    {
        private readonly List<BaseService> _services = new List<BaseService>();
        private ProjectilePool _projectilePool;
        private EnemyPool _enemyPool;
        
        [Inject]
        private void Construct(InputService inputService,ScriptableProjectSettings projectSettings, ProjectilePool projectilePool, EnemyPool enemyPool)
        {
            _services.Add(inputService);
            _projectilePool = projectilePool;
            _projectilePool.Init(projectSettings.poolSize);
            _projectilePool.OnEnemyHit += HitEnemy;

            _enemyPool = enemyPool;
            _enemyPool.Init(projectSettings.poolSize);
        }

        private void Start()
        {
            _enemyPool.SpawnAtPoints();
        }

        private void HitEnemy(UnitEnemy enemy, Projectile projectile)
        {
            Debug.Log(enemy + "     " + projectile);
            _projectilePool.Return(projectile);
        }
        
        private void Update()
        {
            foreach (var service in _services)
            {
                service.Update();
            }
        }
        private void OnEnable()
        {
            foreach (var service in _services)
            {
                service.OnEnable();
            }
        }
        private void OnDisable()
        {
            foreach (var service in _services)
            {
                service.OnDisable();
            }
        }
    }
}