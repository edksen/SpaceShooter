using System;
using System.Collections.Generic;
using MovementSystem;
using MovementSystem.Contracts;
using SpaceShooter.ArmorSystem.Contracts;
using SpaceShooter.ArmorSystem.Settings;
using Entities;
using SpaceShooter.PlayableObjects.Configuration;
using UnityEngine;

namespace SpaceShooter.PlayableObjects
{
    public class Asteroid : MonoBehaviour, IMovableEntity, IDestroyableEntity, IArmoredEntity
    {
        [SerializeField] private AsteroidConfiguration _asteroidConfiguration;
        private List<ArmorConfiguration> _armorConfigurations;

        public event Action OnDestroyEntity;
        public float GasForce => _asteroidConfiguration.GasForce;
        public float MaxSpeed => _asteroidConfiguration.MaxSpeed;
        public float Inertia => 0;
        public float RotationSpeed => _asteroidConfiguration.RotationSpeed;
        public EntityBorderState BorderState => _asteroidConfiguration.AsteroidBorderState;
        public Transform Transform => transform;
        public MonoBehaviour CoroutineRunner => this;

        public List<ArmorConfiguration> Armors
        {
            get
            {
                if(_armorConfigurations == null || _armorConfigurations.Count == 0)
                    _armorConfigurations = new List<ArmorConfiguration> {_asteroidConfiguration.ArmorConfiguration};

                return _armorConfigurations;
            }
        }

        public Transform ArmorTransform => transform;

        private void OnDestroy()
        {
            OnDestroyEntity?.Invoke();
        }
    }
}