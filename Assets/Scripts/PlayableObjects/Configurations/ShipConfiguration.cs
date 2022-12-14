using System.Collections.Generic;
using MovementSystem;
using ArmorSystem.Settings;
using UnityEngine;

namespace SpaceShooter.PlayableObjects.Configuration
{
    [CreateAssetMenu(fileName = "ShipConfiguration", menuName = "Ship Configuration", order = 0)]
    public class ShipConfiguration : ScriptableObject
    {
        public float GasForce;
        public float MaxSpeed;
        public float RotationSpeed;
        [Range(0, 1)] public float Inertia;
        public EntityBorderState ShipBorderState;

        public List<ArmorConfiguration> ArmorConfigurations;
    }
}