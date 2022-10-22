using System.Collections;
using SpaceShooter.AIModule.Contracts;
using SpaceShooter.ArmorSystem;
using SpaceShooter.ArmorSystem.Contracts;
using UnityEngine;

namespace SpaceShooter.AIModule.Entity
{
    public class AIChasingArmoredEntity<T> : IAIEntity where T: MonoBehaviour
    {
        private const AIMovingType MOVING_TYPE = AIMovingType.Chasing;
        private const ArmorType ARMOR_TYPE = ArmorType.Bullet;
        
        private const float CHASING_RANGE = 3f;
        private const float SECONDS_TO_NEXT_SHOT = 2f;
        
        public AIMovingType MovingType => MOVING_TYPE;
        public ArmorType ArmorType => ARMOR_TYPE;

        private readonly Vector3 _relativePosition;
        private T _aiEntity;
        private ChasingEntityMovementController _movementController;
        private IArmoryController _armorController;
        private Vector2 _enemyPosition;
        private bool _madeShot;

        public AIChasingArmoredEntity(
            ChasingEntityMovementController movementController, 
            IArmoryController armoryController,
            Vector3 relativePosition,
            T obj)
        {
            _movementController = movementController;
            _armorController = armoryController;
            _relativePosition = relativePosition;
            _aiEntity = obj;
        }

        public void Run()
        {
            _aiEntity.StartCoroutine(MakeShot());
            _aiEntity.StartCoroutine(Chase());
        }

        public void SetMovementDirection(Vector2 direction)
        {
            _enemyPosition = direction;
        }

        public void OnAIEntityDestroy()
        {
            _movementController = null;
            _armorController = null;
            _aiEntity.StopAllCoroutines();
            _aiEntity = null;
        }

        private IEnumerator Chase()
        {
            while (true)
            {
                var direction = _enemyPosition - (Vector2) _aiEntity.transform.TransformPoint(_relativePosition);

                if (direction.magnitude < CHASING_RANGE)
                {
                    _movementController.MoveEntity(Vector2.zero);
                }
                else
                    _movementController.MoveEntity(direction);
                
                yield return null;
            }
        }

        private IEnumerator MakeShot()
        {
            while (true)
            {
                yield return new WaitUntil(() => _movementController.CanMakeShot && !_madeShot);

                _madeShot = true;
                _armorController?.MakeShot(ArmorType);

                yield return new WaitForSeconds(SECONDS_TO_NEXT_SHOT);
            
                _madeShot = false;
            }
        }
    }
}