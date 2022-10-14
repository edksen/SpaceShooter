using TestShooter.Controllers;
using TestShooter.Entities;
using UnityEngine;

namespace TestShooter.Settings
{
    [CreateAssetMenu(fileName = "ShipConfiguration", menuName = "Ship Configuration", order = 0)]
    public class ShipConfiguration : ScriptableObject
    {
        public float GasForce;
        public float MaxSpeed;
        public float RotationSpeed;
        public float Inertia;
        public EntityBorderState ShipBorderState;

        public Projectile ShipOrdinaryProjectile;
        public Projectile ShipLaserProjectile;
    }
}