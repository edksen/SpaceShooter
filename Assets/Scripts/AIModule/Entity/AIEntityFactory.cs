using System.Collections.Generic;
using System.ComponentModel;
using MovementSystem;
using MovementSystem.Contracts;
using AIModule.Contracts;
using ArmorSystem;
using Entities;
using SpaceShooter.PlayableObjects;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace AIModule.Entity
{
    public class AIEntityFactory
    {
        private List<Asteroid> _randomMovingEntitiesPrefabs;
        private List<SpaceShip> _chasingEntitiesPrefabs;
        private readonly IBorderController _borderController;
        private ArmorFactory _armorFactory;

        public AIEntityFactory(IBorderController borderController, ArmorFactory armorFactory)
        {
            _borderController = borderController;
            _armorFactory = armorFactory;
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
            Asteroid entity = Object.Instantiate(_randomMovingEntitiesPrefabs[Random.Range(0, _randomMovingEntitiesPrefabs.Count - 1)]);
            entity.Transform.position = _borderController.GetRandomPointInBorder();
            
            var movingController = new RegularEntityMovementController(entity, _borderController);
            var armoryController = new EntityArmorController(entity, entity.Armors, _armorFactory);

            var randomMovingAIEntity = new AIRandomMovingBombEntity(movingController, armoryController, entity.gameObject);

            var directionVector = Random.insideUnitCircle.normalized;
            randomMovingAIEntity.SetMovementDirection(directionVector);

            return randomMovingAIEntity;
        }

        private IAIEntity CreateRandomChasingArmoredEntity()
        {
            SpaceShip entity = Object.Instantiate(_chasingEntitiesPrefabs[Random.Range(0, _chasingEntitiesPrefabs.Count - 1)]);
            entity.Transform.position = _borderController.GetRandomPointInBorder();
            
            var movingController = new ChasingEntityMovementController(entity, _borderController);
            var armoryController = new EntityArmorController(entity, entity.Armors, _armorFactory);
            var chasingArmoredEntity = new AIChasingArmoredEntity<SpaceShip>(movingController, armoryController, entity);

            return chasingArmoredEntity;
        }
    }
}