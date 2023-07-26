using Base;
using Base.Interfaces;
using Enemy;
using Mono;
using UnityEngine;
using Zenject;

namespace Infrastructure.Pools.Supply
{
    public class SupplyFactory : IBaseFactory<Mono.Supply>
    {
        private readonly DiContainer _container;
        private readonly GameObject _supplyPrefab;

        public SupplyFactory(GameObject supplyPrefab, DiContainer container)
        {
            _supplyPrefab = supplyPrefab;
            _container = container;
        }

        public Mono.Supply Create(Transform parent)
        {
            return _container.InstantiatePrefabForComponent<Mono.Supply>(_supplyPrefab, parent);
        }
    }
}