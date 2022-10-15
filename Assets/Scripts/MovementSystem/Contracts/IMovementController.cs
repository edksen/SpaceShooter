using UnityEngine;

namespace SpaceShooter.MovingSystem.Contracts
{
    public interface IMovementController
    {
        void MoveEntity(Vector2 direction, bool withoutInertia = false);
    }
}