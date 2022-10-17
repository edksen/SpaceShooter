using System;
using UnityEngine;

namespace SpaceShooter.MovingSystem.Contracts
{
    public interface IMovementController
    {
        event Action<Vector2> OnPositionChanged;
        void MoveEntity(Vector2 direction, bool withoutInertia = false);
    }
}