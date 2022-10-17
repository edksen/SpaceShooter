using MovementSystem;
using UnityEngine;

namespace SpaceShooter.ArmorSystem.Contracts
{
    public class BombArmor : Armor
    {
        public override int AmmoLeft => _bombCount;
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
                var projectile = Object.Instantiate(_projectile);
                projectile.transform.SetPositionAndRotation(_armorTransform.position, _armorTransform.rotation);
                var movementController = new RegularEntityMovementController(projectile, new BorderController());
                movementController.MoveEntity(Random.insideUnitCircle.normalized);
            }
        }
    }
}