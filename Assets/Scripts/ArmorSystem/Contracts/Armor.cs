using System;
using ArmorSystem.Armors;
using Entities;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ArmorSystem.Contracts
{
    public abstract class Armor
    {
        public ArmorType ArmorType => _armorType;

        protected virtual event Action<int, float> _onArmorStatusUpdated;
        public virtual event Action OnProjectileHit;
        
        protected PlaygroundObjectObserver ObjectObserver => PlaygroundObjectObserver.Instance;
        protected virtual int AmmoLeft => Int32.MaxValue;
        protected virtual float AmmoCooldown => 0;
        protected readonly Transform _armorTransform;
        protected readonly Projectile _projectile;
        
        private readonly ArmorType _armorType;

        protected Armor(Projectile projectile, Transform armorTransform, ArmorType armorType)
        {
            _projectile = projectile;
            _armorType = armorType;
            _armorTransform = armorTransform;
        }
        
        public void SetOnArmorStatusUpdated(Action<int, float> onArmorStatusUpdated)
        {
            _onArmorStatusUpdated += onArmorStatusUpdated;
            onArmorStatusUpdated.Invoke(AmmoLeft, AmmoCooldown);
        }
        
        public virtual void OnArmorDestroy(){}

        internal virtual void MakeShot()
        {
            CreateProjectile();
        }

        protected virtual Projectile CreateProjectile()
        {
            Projectile projectile = Object.Instantiate(_projectile);
            projectile.Transform.SetPositionAndRotation(_armorTransform.position, _armorTransform.rotation);
            projectile.OnDestroyCaughtEntity += ObjectObserver.DestroyEntity;

            return projectile;
        }
    }
}