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
        public GameObject EntityObject => _entityObject;
        public ArmorType ArmorType => ArmorType.Bomb;

        private MovementControllerBase _movementController;
        private IArmoryController _armorController;
        private Vector2 _movementDirection;
        private GameObject _entityObject;

        public AIRandomMovingBombEntity(MovementControllerBase movementController, IArmoryController armoryController, GameObject entityObject)
        {
            _movementController = movementController;
            _armorController = armoryController;
            _entityObject = entityObject;
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
            _armorController.MakeShot(ArmorType.Bomb);
            _movementController.OnEntityDestroyed();
            _armorController.OnEntityDestroyed();
            _movementController = null;
            _armorController = null;
            _entityObject = null;
        }
    }
}