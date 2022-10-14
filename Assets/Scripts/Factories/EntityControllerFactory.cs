using Controllers.EntityControllers;
using TestShooter.Controllers;
using TestShooter.Entities;
using UnityEngine;

namespace TestShooter.GameCore.Factories
{
    public class EntityControllerFactory : IEntityControllerFactory
    {
        private readonly IBorderController _borderController;

        public EntityControllerFactory(IBorderController borderController)
        {
            _borderController = borderController;
        }
        
        public IMovementController CreateMovingController<T>(T gameObject) where T : MonoBehaviour, IMovableEntity
        {
            return new EntityMovementController(gameObject, _borderController);
        }

        public IArmoryController CreateArmoryController<T>(T gameObject, IEntityFactory projectileFactory) where T : MonoBehaviour, IArmoredEntity
        {
            return new EntityArmorController(gameObject, projectileFactory);
        }

        public T InstantiateEntity<T>(T gameObject, Transform parentTransform) where T : MonoBehaviour
        {
            return GameObject.Instantiate(gameObject, parentTransform);
        }
    }
}