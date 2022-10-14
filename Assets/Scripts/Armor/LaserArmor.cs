using System;
using TestShooter.Controllers;
using TestShooter.Entities;
using TestShooter.GameCore.Factories;
using UnityEngine;

namespace TestShooter.GameCore.Armor
{
    public class LaserArmor : Armor
    {
        private int _maxShots;
        private int _currentShots;
        private float _cooldownRate;
        
        public LaserArmor(Projectile projectile, Transform armoryPosition) : base(projectile, armoryPosition, ArmorType.Laser){}
        
        public override void MakeShot(IEntityFactory projectileCreator, bool directional = true)
        {
            projectileCreator.InstantiateEntity(_armorProjectile, _armorTransform);
        }
    }
}