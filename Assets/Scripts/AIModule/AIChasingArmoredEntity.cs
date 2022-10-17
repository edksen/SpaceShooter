using MovementSystem.Contracts;
using SpaceShooter.AIModule.Contracts;
using SpaceShooter.ArmorSystem;
using SpaceShooter.ArmorSystem.Contracts;
using UnityEngine;

namespace SpaceShooter.AIModule
{
    public class AIChasingArmoredEntity : IAIEntity
    {
        private const AIMovingType MOVING_TYPE = AIMovingType.Chasing;
        public AIMovingType MovingType => MOVING_TYPE;

        private Transform _entityTransform;
        private Vector3 _relativePosition;
        private MovementControllerBase _movementController;
        private IArmoryController _armorController;
        private Vector2 _enemyPosition;
        
        private ArmorType AIEntityArmorType => ArmorType.Bullet;

        public AIChasingArmoredEntity(MovementControllerBase movementController, IArmoryController armoryController, Transform entityTransform, Vector3 relativePosition)
        {
            _entityTransform = entityTransform;
            _movementController = movementController;
            _armorController = armoryController;
            _relativePosition = relativePosition;
        }

        public void Run(Vector2 position)
        {
            _enemyPosition = position;
            var direction = Chase();
            
            if(direction == (Vector2) _entityTransform.up)
                _armorController.MakeShot(AIEntityArmorType);
        }

        public void OnAIEntityDestroy()
        {
            _entityTransform = null;
            _movementController = null;
            _armorController = null;
        }

        private Vector2 Chase()
        {
            var direction = (_enemyPosition - (Vector2)_entityTransform.TransformPoint(_relativePosition)).normalized;
            _movementController.MoveEntity(direction);

            return direction;
        }
    }
}