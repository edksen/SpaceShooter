using Controllers.EntityControllers;
using SpaceShooter.ArmorSystem.Contracts;
using SpaceShooter.Controllers;
using UnityEngine;

namespace SpaceShooter.ArmorSystem
{
    public class LightArmor : Armor
    {
        public LightArmor(Projectile projectile, Transform armorPosition) : base(projectile, armorPosition, ArmorType.Direct){}

        public override void MakeShot()
        {
            Projectile projectile = Object.Instantiate(_projectile);
            projectile.Transform.SetPositionAndRotation(_armorTransform.position, _armorTransform.rotation);
            
            new EntityMovementController(projectile, new BorderController()).MoveEntity(new Vector2(0, 1), true);
        }
    }
}