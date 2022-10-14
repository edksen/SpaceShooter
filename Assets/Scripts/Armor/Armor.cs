using TestShooter.Entities;
using TestShooter.GameCore.Factories;
using UnityEngine;

namespace TestShooter.GameCore.Armor
{
    public abstract class Armor
    {
        private ArmorType _armorType;
        
        protected Projectile _armorProjectile;
        protected Transform _armorTransform;
        public ArmorType ArmorType => _armorType;
        
        protected Armor(Projectile projectile, Transform armorTransform, ArmorType armorType)
        {
            _armorProjectile = projectile;
            _armorType = armorType;
            _armorTransform = armorTransform;
        }
        
        public abstract void MakeShot(IEntityFactory projectileCreator, bool directional = true);
    }
}