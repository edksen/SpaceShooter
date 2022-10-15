using UnityEngine;

namespace SpaceShooter.ArmorSystem.Contracts
{
    public abstract class Armor
    {
        private readonly ArmorType _armorType;

        protected readonly Projectile _projectile;
        protected readonly Transform _armorTransform;

        public ArmorType ArmorType => _armorType;
        
        protected Armor(Projectile projectile, Transform armorTransform, ArmorType armorType)
        {
            _projectile = projectile;
            _armorType = armorType;
            _armorTransform = armorTransform;
        }

        public virtual void MakeShot()
        {
            Object.Instantiate(_projectile, _armorTransform);
        }
    }
}