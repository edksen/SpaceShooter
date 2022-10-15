using UnityEngine;

namespace SpaceShooter.Controllers
{
    public interface IMovementController
    {
        void MoveEntity(Vector2 direction, bool withoutInertia = false);
    }
}