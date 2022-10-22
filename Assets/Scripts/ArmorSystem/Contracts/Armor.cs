using ArmorSystem.Armors;
using UnityEngine;

namespace ArmorSystem.Contracts
{
    public abstract class Armor
    {
        private readonly ArmorType _armorType;

        protected readonly Projectile _projectile;
        protected readonly Transform _armorTransform;

        public ArmorType ArmorType => _armorType;
        public abstract int AmmoLeft { get; }
        
        protected Armor(Projectile projectile, Transform armorTransform, ArmorType armorType)
        {
            _projectile = projectile;
            _armorType = armorType;
            _armorTransform = armorTransform;
        }

        internal abstract void MakeShot();
    }
}