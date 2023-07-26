using System;
using Base;
using Base.Classes;
using Base.Interfaces;
using UnityEngine;
using Zenject;

namespace Projectile
{
    public class ProjectileView : BaseView, IProceduralView
    {
        [SerializeField] private Rigidbody rigidBody;
        public Rigidbody Rigidbody => rigidBody;

        private ProjectilePresenter _presenter;

        [Inject]
        private void Construct()
        {
            
        }
        
        private void OnCollisionEnter(Collision collision)
        {
            _presenter.Hit(collision);
        }

        public void SetPresenter(BasePresenter presenter)
        {
            _presenter = (ProjectilePresenter)presenter;
        }

        private void OnDestroy()
        {
            _presenter.Dispose();
        }
    }
}