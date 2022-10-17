using System;
using System.Collections.Generic;
using System.Linq;
using MovementSystem;
using MovementSystem.Contracts;
using SpaceShooter.AIModule.Configuration;
using SpaceShooter.AIModule.Contracts;
using SpaceShooter.ArmorSystem;
using SpaceShooter.PlayableObjects;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace SpaceShooter.AIModule
{
    public class AIController : IAIController
    {
        private readonly HashSet<IAIEntity> _entities;
        private readonly IBorderController _borderController;
        private readonly Transform _objectPool;
        
        private readonly List<SpaceShip> _chasingEntitiesList;
        private readonly List<Asteroid> _randomMovingEntitiesList;

        private readonly int _maxAIEntityType;
        private int _maxEntitiesOnPlayground;
        private readonly int _secsToAddAnotherEntity;

        private float _secsFromGameStart;

        public AIController(
            IBorderController borderController, 
            AIControllerConfiguration controllerConfiguration, 
            Transform objectPool)
        {
            _chasingEntitiesList = controllerConfiguration.ChasingArmoredEntity;
            _randomMovingEntitiesList = controllerConfiguration.RandomMovingEntity;
            _maxEntitiesOnPlayground = controllerConfiguration.StartMaxEntitiesOnPlayGround;
            _secsToAddAnotherEntity = controllerConfiguration.SecsToAddEntity;
            
            _borderController = borderController;
            _objectPool = objectPool;

            _entities = new HashSet<IAIEntity>();
            _maxAIEntityType = Enum.GetValues(typeof(AIMovingType)).Cast<int>().Max();
            _secsFromGameStart = 0f;
        }

        public void Start(Vector2 playerStartPosition)
        {
            FullEntities();
            ChaseEnemy(playerStartPosition);
        }

        public void ControllerUpdate()
        {
            /*_secsFromGameStart += Time.deltaTime;

            int timeMod = (int) _secsFromGameStart % _secsToAddAnotherEntity;
            if (timeMod >= 1)
            {
                ++_maxEntitiesOnPlayground;
                _secsFromGameStart = 0;
                CreateEntity();
            }*/
        }

        public void ChaseEnemy(Vector2 enemyPosition)
        {
            foreach (var entity in _entities)
            {
                if(entity.MovingType == AIMovingType.Chasing)
                    entity.Run(enemyPosition);
            }
        }

        private void FullEntities()
        {
            while (_entities.Count < _maxEntitiesOnPlayground)
            {
                CreateEntity();
            }
        }

        private void CreateEntity()
        {
            AIMovingType aiEntityTypeToCreate = (AIMovingType) Random.Range(0, _maxAIEntityType + 1);

            IAIEntity createdEntity = null;
            switch (aiEntityTypeToCreate)
            {
                case AIMovingType.Chasing:
                    createdEntity = CreateRandomChasingArmoredEntity();
                    break;
                case AIMovingType.Random:
                    createdEntity = CreateRandomMovingEntity();
                    break;
                default:
                    break;
            }

            if (createdEntity != null)
                _entities.Add(createdEntity);
        }

        private IAIEntity CreateRandomMovingEntity()
        {
            Asteroid entity = Object.Instantiate(_randomMovingEntitiesList[Random.Range(0, _randomMovingEntitiesList.Count - 1)], _objectPool.transform);
            entity.Transform.position = _borderController.GetRandomPointInBorder();
            
            var movingController = new RegularEntityMovementController(entity, _borderController);
            var armoryController = new EntityArmorController(entity, entity.Armors);

            var randomMovingAIEntity = new AIRandomMovingEntity(movingController, armoryController);
            entity.OnDestroyEntity += () =>
            {
                randomMovingAIEntity.OnAIEntityDestroy();
                OnEntityDestroyed(randomMovingAIEntity);
            };

            entity.name += _entities.Count;
            var directionVector = Random.insideUnitCircle.normalized;
            
            Debug.Log($"For entity: {entity.name} setting direction: {directionVector}");
            
            movingController.MoveEntity(directionVector);

            return randomMovingAIEntity;
        }

        private IAIEntity CreateRandomChasingArmoredEntity()
        {
            SpaceShip entity = Object.Instantiate(_chasingEntitiesList[Random.Range(0, _chasingEntitiesList.Count - 1)], _objectPool.transform);
            entity.Transform.position = _borderController.GetRandomPointInBorder();
            
            var movingController = new RegularEntityMovementController(entity, _borderController);
            var armoryController = new EntityArmorController(entity, entity.Armors);
            var chasingArmoredEntity = new AIChasingArmoredEntity(movingController, armoryController, entity.transform, _objectPool.transform.position);

            entity.OnDestroyEntity += () =>
            {
                chasingArmoredEntity.OnAIEntityDestroy();
                OnEntityDestroyed(chasingArmoredEntity);
            };
            
            return chasingArmoredEntity;
        }

        private void OnEntityDestroyed(IAIEntity destroyedEntity)
        {
            CreateEntity();
            if (_entities.Contains(destroyedEntity))
                _entities.Remove(destroyedEntity);
        }
    }
}