using Controllers.EntityControllers;
using TestShooter.Controllers;
using TestShooter.Entities;
using UnityEngine;

namespace TestShooter.GameCore.Factories
{
    public interface IEntityControllerFactory
    {
        IMovementController CreateMovingController<T>(T gameObject) where T : MonoBehaviour, IMovableEntity;

        IArmoryController CreateArmoryController<T>(T gameObject, IEntityFactory projectileFactory) where T : MonoBehaviour, IArmoredEntity;
    }
}