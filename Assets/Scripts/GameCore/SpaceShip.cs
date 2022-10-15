using System;
using System.Collections.Generic;
using SpaceShooter.ArmorSystem;
using SpaceShooter.ArmorSystem.Contracts;
using SpaceShooter.Controllers;
using SpaceShooter.Entities;
using SpaceShooter.GameCore.Configuration;
using UnityEngine;

namespace SpaceShooter.GameCore
{
    public class SpaceShip : MonoBehaviour, IMovableEntity, IArmoredEntity, IDestroyableEntity
    {
        [SerializeField] private ShipConfiguration _shipConfiguration;
        [SerializeField] private Transform _armoryPosition;
        
        private List<Armor> _armors;
        
        public event Action OnDestroyEntity;
        public float GasForce => _shipConfiguration.GasForce;
        public float MaxSpeed => _shipConfiguration.MaxSpeed;
        public float Inertia => _shipConfiguration.Inertia;
        public float RotationSpeed => _shipConfiguration.RotationSpeed;
        public EntityBorderState BorderState => _shipConfiguration.ShipBorderState;
        public List<Armor> Armors => _armors;
        public Transform Transform => transform;
        public MonoBehaviour CoroutineRunner => this;

        public void SetArmor(List<Armor> armors)
        {
            
        }

        private void Awake()
        {
            _armors = new List<Armor>()
            {
                new LightArmor(_shipConfiguration.shipOrdinaryProjectile, _armoryPosition), 
                new LaserArmor(_shipConfiguration.shipLaserProjectile, _armoryPosition)
            };
        }

        private void OnDestroy()
        {
            OnDestroyEntity?.Invoke();
        }
    }
}