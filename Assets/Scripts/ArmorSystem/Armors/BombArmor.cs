using ArmorSystem.Contracts;
using Entities;
using MovementSystem;
using UnityEngine;

namespace ArmorSystem.Armors
{
    public class BombArmor : Armor
    {
        private readonly int _attackRate;
        private int _bombCount;
        
        public BombArmor(Projectile projectile, Transform armorTransform, int attackRate, int ammoCapacity, PlaygroundObjectObserver objectObserver) 
            : base(projectile, armorTransform, ArmorType.Bomb, objectObserver)
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
                
                _objectObserver.SetOnDestroyAction(projectile.gameObject, () =>
                {
                    Object.Destroy(projectile.gameObject);
                    movementController.OnEntityDestroyed();
                });
            }
        }
    }
}