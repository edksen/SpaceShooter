using System;
using SpaceShooter.AIModule.Contracts;
using SpaceShooter.ArmorSystem;
using SpaceShooter.ArmorSystem.Contracts;
using SpaceShooter.MovingSystem.Contracts;
using UnityEngine;

namespace SpaceShooter.AIModule
{
    public class AIChasingArmoredEntity : IAIEntity
    {
        private const AIMovingType MOVING_TYPE = AIMovingType.Chasing;
        public AIMovingType MovingType => MOVING_TYPE;

        private Transform _entityTransform;
        private IMovementController _movementController;
        private IArmoryController _armorController;
        
        private ArmorType AIEntityArmorType => ArmorType.Bullet;

        public AIChasingArmoredEntity(IMovementController movementController, IArmoryController armoryController, Transform entityTransform)
        {
            _entityTransform = entityTransform;
            _movementController = movementController;
            _armorController = armoryController;
        }

        public void Run(Vector2 position)
        {
            var direction = ((Vector2) _entityTransform.position - position).normalized;
            _movementController.MoveEntity(direction);
            
            if(direction == (Vector2) _entityTransform.up)
                _armorController.MakeShot(AIEntityArmorType);
        }

        public void OnAIEntityDestroy()
        {
            _entityTransform = null;
            _movementController = null;
            _armorController = null;
        }
    }
}