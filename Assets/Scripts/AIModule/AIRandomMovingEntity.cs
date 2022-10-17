using SpaceShooter.AIModule.Contracts;
using SpaceShooter.ArmorSystem;
using SpaceShooter.ArmorSystem.Contracts;
using SpaceShooter.MovingSystem.Contracts;
using UnityEngine;

namespace SpaceShooter.AIModule
{
    public class AIRandomMovingEntity : IAIEntity
    {
        private const AIMovingType MOVING_TYPE = AIMovingType.Random;
        public AIMovingType MovingType => MOVING_TYPE;
        
        private IMovementController _movementController;
        private IArmoryController _armorController;
        
        public AIRandomMovingEntity(IMovementController movementController, IArmoryController armoryController)
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