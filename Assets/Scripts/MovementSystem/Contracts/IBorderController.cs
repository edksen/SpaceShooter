using UnityEngine;

namespace MovementSystem.Contracts
{
    public interface IBorderController
    {
        void CheckEntity(IMovableEntity entity, Vector2 currentDirection);

        Vector2 GetRandomPointInBorder();
    }
}