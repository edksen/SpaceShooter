using SpaceShooter.Entities;
using UnityEngine;

namespace Controllers.EntityControllers
{
    public interface IBorderController
    {
        void CheckEntity(IMovableEntity entity, Vector2 currentDirection);
    }
}