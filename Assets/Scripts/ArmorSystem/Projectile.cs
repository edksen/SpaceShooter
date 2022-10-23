using System;
using MovementSystem;
using MovementSystem.Contracts;
using ArmorSystem.Settings;
using Entities;
using UnityEngine;

namespace ArmorSystem
{
    public class Projectile : MonoBehaviour, IMovableEntity, IDestroyableEntity
    {
        [SerializeField] private ProjectileConfiguration _projectileConfiguration;

        private float _timeLeft = -1;
        
        public event Action<GameObject> OnDestroyCaughtEntity;
        public float GasForce => MaxSpeed;
        public float MaxSpeed => _projectileConfiguration.MaxSpeed;
        public float Inertia => _projectileConfiguration.Inertia;
        public float RotationSpeed => 0;
        public Transform Transform => transform;
        public EntityBorderState BorderState => _projectileConfiguration.BorderState;
        public float LifeTime => _projectileConfiguration.LifeTimeSec;

        private void OnTriggerEnter2D(Collider2D col)
        {
            if(_projectileConfiguration.DestroyOnHit)
                OnDestroyCaughtEntity?.Invoke(gameObject);
            
            OnDestroyCaughtEntity?.Invoke(col.gameObject);
        }
    }
}