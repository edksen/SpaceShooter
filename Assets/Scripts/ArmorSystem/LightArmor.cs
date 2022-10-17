using System;
using Controllers.EntityControllers;
using SpaceShooter.ArmorSystem.Contracts;
using SpaceShooter.MovingSystem;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SpaceShooter.ArmorSystem
{
    public class LightArmor : Armor
    {
        public override int AmmoLeft => Int32.MaxValue;
        
        public LightArmor(Projectile projectile, Transform armorPosition) : base(projectile, armorPosition, ArmorType.Bullet){}

        internal override void MakeShot()
        {
            Projectile projectile = Object.Instantiate(_projectile);
            projectile.Transform.SetPositionAndRotation(_armorTransform.position, _armorTransform.rotation);
            
            new EntityMovementController(projectile, new BorderController()).MoveEntity(new Vector2(0, 1), true);
        }
    }
}