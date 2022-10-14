using UnityEngine;

namespace TestShooter.Controllers
{
    public interface IMovementController
    {
        void MoveEntity(Vector2 direction, bool withoutInertia = false);
    }
}