using System;
using System.Collections.Generic;
using MovementSystem;
using ArmorSystem.Contracts;
using MovementSystem.Contracts;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ArmorSystem.Armors
{
    public class LightArmor : Armor
    {
        private List<MovementControllerBase> _projectileMovementControllers;
        public override event Action OnProjectileHit;

        public LightArmor(Projectile projectile, Transform armorPosition) : base(projectile, armorPosition,
            ArmorType.Bullet)
        {
            _projectileMovementControllers = new List<MovementControllerBase>();
        }

        protected override Projectile CreateProjectile()
        {
            var projectile = base.CreateProjectile();
            
            var projectileController = new RegularEntityMovementController(projectile, new BorderController());
            
            _projectileMovementControllers.Add(projectileController);

            projectile.OnDestroyCaughtEntity += gameObject =>
            {
                ObjectObserver.DestroyEntity(gameObject);
                if(gameObject != projectile.gameObject)
                    OnProjectileHit?.Invoke();
            };
            
            ObjectObserver.SetOnDestroyAction(projectile.gameObject, () =>
            {
                if(projectile)
                    Object.Destroy(projectile.gameObject);
                
                projectileController.OnEntityDestroyed();
                _projectileMovementControllers.Remove(projectileController);
            });
            
            projectileController.MoveEntity(new Vector2(0, 1));

            return projectile;
        }
    }
}