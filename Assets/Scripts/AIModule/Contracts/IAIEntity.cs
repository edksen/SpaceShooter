using System;
using UnityEngine;

namespace SpaceShooter.AIModule.Contracts
{
    public interface IAIEntity
    {
        AIMovingType MovingType { get; }
        void Run(Vector2 position);
        void OnAIEntityDestroy();
    }
}