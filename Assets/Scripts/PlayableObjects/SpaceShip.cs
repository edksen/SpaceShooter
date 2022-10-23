using System;
using System.Collections.Generic;
using MovementSystem;
using MovementSystem.Contracts;
using ArmorSystem.Contracts;
using ArmorSystem.Settings;
using Entities;
using SpaceShooter.PlayableObjects.Configuration;
using UnityEngine;

namespace SpaceShooter.PlayableObjects
{
    public class SpaceShip : MonoBehaviour, IMovableEntity, IArmoredEntity, IDestroyableEntity
    {
        [SerializeField] private ShipConfiguration _shipConfiguration;
        [SerializeField] private Transform _armoryPosition;
        
        private List<Armor> _armors;
        
        public event Action<GameObject> OnDestroyCaughtEntity;
        public float GasForce => _shipConfiguration.GasForce;
        public float MaxSpeed => _shipConfiguration.MaxSpeed;
        public float Inertia => _shipConfiguration.Inertia;
        public float RotationSpeed => _shipConfiguration.RotationSpeed;
        public EntityBorderState BorderState => _shipConfiguration.ShipBorderState;
        public List<ArmorConfiguration> Armors => _shipConfiguration.ArmorConfigurations;
        public Transform Transform => transform;
        public Transform ArmorTransform => _armoryPosition;

        private void OnTriggerEnter2D(Collider2D col)
        {
            OnDestroyCaughtEntity?.Invoke(gameObject);
            OnDestroyCaughtEntity?.Invoke(col.gameObject);
        }
    }
}