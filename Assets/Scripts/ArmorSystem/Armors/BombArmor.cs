using ArmorSystem.Contracts;
using MovementSystem;
using UnityEngine;

namespace ArmorSystem.Armors
{
    public class BombArmor : Armor
    {
        protected override int AmmoLeft => _bombCount;
        protected override float AmmoCooldown => 0;
        private readonly int _attackRate;
        private int _bombCount;
        
        public BombArmor(Projectile projectile, Transform armorTransform, int attackRate, int ammoCapacity) : base(projectile, armorTransform, ArmorType.Bomb)
        {
            _bombCount = ammoCapacity;
            _attackRate = attackRate;
        }
        
        internal override void MakeShot()
        {
            if(_bombCount == 0)
                return;
            
            --_bombCount;
            for (int i = 0; i < _attackRate; ++i)
            {
                var projectile = CreateProjectile();

                var movementController = new RegularEntityMovementController(projectile, new BorderController());
                movementController.MoveEntity(Random.insideUnitCircle.normalized);
                
                ObjectObserver.SetOnDestroyAction(projectile.gameObject, () =>
                {
                    Object.Destroy(projectile.gameObject);
                    movementController.OnEntityDestroyed();
                });
            }
        }
    }
}