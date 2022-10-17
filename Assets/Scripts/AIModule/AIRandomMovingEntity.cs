using MovementSystem.Contracts;
using SpaceShooter.AIModule.Contracts;
using SpaceShooter.ArmorSystem;
using SpaceShooter.ArmorSystem.Contracts;
using UnityEngine;

namespace SpaceShooter.AIModule
{
    public class AIRandomMovingEntity : IAIEntity
    {
        private const AIMovingType MOVING_TYPE = AIMovingType.Random;
        public AIMovingType MovingType => MOVING_TYPE;
        
        private MovementControllerBase _movementController;
        private IArmoryController _armorController;
        
        public AIRandomMovingEntity(MovementControllerBase movementController, IArmoryController armoryController)
        {
            _movementController = movementController;
            _armorController = armoryController;
        }
        
        public void Run(Vector2 position)
        {
            _movementController.MoveEntity(position);
        }

        public void OnAIEntityDestroy()
        {
            _movementController = null;
            _armorController.MakeShot(ArmorType.Bomb);
            _armorController = null;
        }
    }
}