using System.Collections.Generic;
using System.ComponentModel;
using MovementSystem;
using MovementSystem.Contracts;
using SpaceShooter.AIModule.Contracts;
using SpaceShooter.ArmorSystem;
using SpaceShooter.PlayableObjects;
using UnityEngine;

namespace SpaceShooter.AIModule.Entity
{
    public class AIEntityFactory
    {
        private List<Asteroid> _randomMovingEntitiesPrefabs;
        private List<SpaceShip> _chasingEntitiesPrefabs;
        private readonly IBorderController _borderController;
        private readonly Transform _objectPool;

        public AIEntityFactory(Transform objectPool, IBorderController borderController)
        {
            _objectPool = objectPool;
            _borderController = borderController;
        }

        public void SetEntitiesLists(List<Asteroid> randomMovingEntitiesPrefabs, List<SpaceShip> chasingEntitiesPrefabs)
        {
            _randomMovingEntitiesPrefabs = randomMovingEntitiesPrefabs;
            _chasingEntitiesPrefabs = chasingEntitiesPrefabs;
        }

        public IAIEntity CreateEntity(AIMovingType entityType)
        {
            switch (entityType)
            {
                case AIMovingType.Chasing:
                    return CreateRandomChasingArmoredEntity();
                case AIMovingType.Random:
                    return CreateRandomMovingBombEntity();
                default:
                    throw new InvalidEnumArgumentException($"Cannot create entity with type: {entityType}");
            }
        }
        
        
        private IAIEntity CreateRandomMovingBombEntity()
        {
            Asteroid entity = Object.Instantiate(_randomMovingEntitiesPrefabs[Random.Range(0, _randomMovingEntitiesPrefabs.Count - 1)], _objectPool.transform);
            entity.Transform.position = _borderController.GetRandomPointInBorder();
            
            var movingController = new RegularEntityMovementController(entity, _borderController);
            var armoryController = new EntityArmorController(entity, entity.Armors);

            var randomMovingAIEntity = new AIRandomMovingBombEntity(movingController, armoryController);
            entity.OnDestroyEntity += () =>
            {
                randomMovingAIEntity.OnAIEntityDestroy();
                armoryController.MakeShot(randomMovingAIEntity.ArmorType);
            };
            
            var directionVector = Random.insideUnitCircle.normalized;
            randomMovingAIEntity.SetMovementDirection(directionVector);

            return randomMovingAIEntity;
        }

        private IAIEntity CreateRandomChasingArmoredEntity()
        {
            SpaceShip entity = Object.Instantiate(_chasingEntitiesPrefabs[Random.Range(0, _chasingEntitiesPrefabs.Count - 1)], _objectPool.transform);
            entity.Transform.position = _borderController.GetRandomPointInBorder();
            
            var movingController = new ChasingEntityMovementController(entity, _borderController);
            var armoryController = new EntityArmorController(entity, entity.Armors);
            var chasingArmoredEntity = new AIChasingArmoredEntity<SpaceShip>(movingController, armoryController, _objectPool.transform.position, entity);

            entity.OnDestroyEntity += () =>
            {
                chasingArmoredEntity.OnAIEntityDestroy();
            };
            
            return chasingArmoredEntity;
        }
    }
}