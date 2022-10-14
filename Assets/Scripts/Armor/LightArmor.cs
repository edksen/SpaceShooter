using TestShooter.Entities;
using TestShooter.GameCore.Factories;
using UnityEngine;

namespace TestShooter.GameCore.Armor
{
    public class LightArmor : Armor
    {
        public LightArmor(Projectile projectile, Transform armorPosition) : base(projectile, armorPosition, ArmorType.Light){}

        public override void MakeShot(IEntityFactory projectileCreator, bool directional = true)
        {
            var movementController = projectileCreator.CreateMovableEntity(_armorProjectile, _armorTransform);

            Vector2 projectileDirection =
                directional ? new Vector2(0, 1) : Random.insideUnitCircle.normalized;

            movementController.MoveEntity(projectileDirection, true);
        }
    }
}