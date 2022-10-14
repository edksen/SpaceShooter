using Controllers.EntityControllers;
using TestShooter.Controllers;
using TestShooter.Entities;
using UnityEngine;

namespace TestShooter.GameCore.Factories
{
    public interface IEntityFactory
    {
        T InstantiateEntity<T>(T gameObject, Transform parentTransform) where T : MonoBehaviour;
        IMovementController CreateMovableEntity<T>(T gameObject, Transform parentTransform) where T : MonoBehaviour, IMovableEntity;

        IArmoryController CreateArmoredEntity<T>(T gameObject, Transform parentTransform) where T : MonoBehaviour, IArmoredEntity;

        T CreateArmoredMovingEntity<T>(T gameObject, Transform parentTransform,
            out IMovementController movementController, out IArmoryController armoryController)
            where T : MonoBehaviour, IArmoredEntity, IMovableEntity;
    }
}