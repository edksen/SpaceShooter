using Controllers.EntityControllers;
using TestShooter.Controllers;
using TestShooter.Entities;
using UnityEngine;

namespace TestShooter.GameCore.Factories
{
    public class EntityFactory : IEntityFactory
    {
        private readonly IEntityControllerFactory _entityControllerFactory;
        private readonly IInstanceCreator _instanceCreator;

        public EntityFactory(IInstanceCreator instanceCreator, IEntityControllerFactory entityControllerFactory)
        {
            _instanceCreator = instanceCreator;
            _entityControllerFactory = entityControllerFactory;
        }
        
        public T InstantiateEntity<T>(T gameObject, Transform parentTransform) where T : MonoBehaviour
        {
            return _instanceCreator.InstantiateObject(gameObject, parentTransform);
        }

        public IMovementController CreateMovableEntity<T>(T gameObject, Transform parentTransform) where T : MonoBehaviour, IMovableEntity
        {
            var instance = InstantiateEntity(gameObject, null);
            instance.Transform.SetPositionAndRotation(parentTransform.position, parentTransform.rotation);
            return _entityControllerFactory.CreateMovingController(instance);
        }

        public IArmoryController CreateArmoredEntity<T>(T gameObject, Transform parentTransform) where T : MonoBehaviour, IArmoredEntity
        {
            var instance = InstantiateEntity(gameObject, parentTransform);
            return _entityControllerFactory.CreateArmoryController(instance, this);
        }

        public T CreateArmoredMovingEntity<T>(T gameObject, Transform parentTransform, out IMovementController movementController,
            out IArmoryController armoryController) where T : MonoBehaviour, IArmoredEntity, IMovableEntity
        {
            var instance = InstantiateEntity(gameObject, parentTransform);
            
            movementController = _entityControllerFactory.CreateMovingController(instance);
            armoryController = _entityControllerFactory.CreateArmoryController(instance, this);

            return instance;
        }
    }
}