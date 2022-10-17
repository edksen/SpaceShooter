using SpaceShooter.ArmorSystem.Settings;
using SpaceShooter.MovingSystem;
using UnityEngine;

namespace SpaceShooter.PlayableObjects.Configuration
{
    [CreateAssetMenu(fileName = "AsteroidConfiguration", menuName = "Asteroid Configuration Asset", order = 0)]
    public class AsteroidConfiguration : ScriptableObject
    {
        public float MaxSpeed;
        public float GasForce;
        public int RotationSpeed;
        public EntityBorderState AsteroidBorderState;

        public ArmorConfiguration ArmorConfiguration;
    }
}