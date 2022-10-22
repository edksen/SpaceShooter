using System;
using MovementSystem;
using MovementSystem.Contracts;
using SpaceShooter.ArmorSystem.Settings;
using Entities;
using UnityEngine;

namespace SpaceShooter.ArmorSystem
{
    public class Projectile : MonoBehaviour, IMovableEntity, IDestroyableEntity
    {
        [SerializeField] private ProjectileConfiguration _projectileConfiguration;

        private float _timeLeft = -1;
        
        public event Action OnDestroyEntity;
        public float GasForce => MaxSpeed;
        public float MaxSpeed => _projectileConfiguration.MaxSpeed;
        public float Inertia => _projectileConfiguration.Inertia;
        public float RotationSpeed => 0;
        public Transform Transform => transform;
        public EntityBorderState BorderState => _projectileConfiguration.BorderState;

        public MonoBehaviour CoroutineRunner => this;

        private void Start()
        {
            if (_projectileConfiguration.LifeTimeSec != 0)
                _timeLeft = _projectileConfiguration.LifeTimeSec;
        }

        private void Update()
        {
            if (_timeLeft > 0)
            {
                _timeLeft -= Time.deltaTime;
                if(_timeLeft < 0)
                    Destroy(gameObject);
            }
        }

        private void OnDestroy()
        { 
            OnDestroyEntity?.Invoke();
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            Destroy(col.gameObject);
            Destroy(gameObject);
        }
    }
}