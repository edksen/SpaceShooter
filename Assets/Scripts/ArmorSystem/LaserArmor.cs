using SpaceShooter.ArmorSystem.Contracts;
using UnityEngine;

namespace SpaceShooter.ArmorSystem
{
    public class LaserArmor : Armor
    {
        private int _maxShots;
        private int _currentShots;
        private float _cooldownRate;
        
        public LaserArmor(Projectile projectile, Transform armoryPosition) : base(projectile, armoryPosition, ArmorType.Static){}
    }
}