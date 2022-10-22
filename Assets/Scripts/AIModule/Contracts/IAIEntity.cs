using ArmorSystem.Armors;
using UnityEngine;

namespace AIModule.Contracts
{
    public interface IAIEntity
    {
        ArmorType ArmorType { get; }
        AIMovingType MovingType { get; }
        void Run();
        void SetMovementDirection(Vector2 direction);
        void OnAIEntityDestroy();
    }
}