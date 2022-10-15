using SpaceShooter.Entities;
using UnityEngine;

namespace SpaceShooter.MovingSystem.Contracts
{
    public interface IBorderController
    {
        void CheckEntity(IMovableEntity entity, Vector2 currentDirection);
    }
}