using MovementSystem.Contracts;
using AIModule.Contracts;
using ArmorSystem.Armors;
using ArmorSystem.Contracts;
using UnityEngine;

namespace AIModule.Entity
{
    public class AIRandomMovingBombEntity : IAIEntity
    {
        public AIMovingType MovingType => AIMovingType.Random;
        public ArmorType ArmorType => ArmorType.Bomb;

        private MovementControllerBase _movementController;
        private IArmoryController _armorController;
        private Vector2 _movementDirection;

        public AIRandomMovingBombEntity(MovementControllerBase movementController, IArmoryController armoryController)
        {
            _movementController = movementController;
            _armorController = armoryController;
        }

        public void SetMovementDirection(Vector2 direction)
        {
            _movementDirection = direction;
        }
        
        public void Run()
        {
            _movementController.MoveEntity(_movementDirection);
        }

        public void OnAIEntityDestroy()
        {
            _movementController = null;
            _armorController.MakeShot(ArmorType.Bomb);
            _armorController = null;
        }
    }
}