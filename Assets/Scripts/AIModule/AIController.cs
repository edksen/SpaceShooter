using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AIModule.Configuration;
using AIModule.Contracts;
using AIModule.Entity;
using Entities;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AIModule
{
    public class AIController : IAIController
    {
        private readonly HashSet<IAIEntity> _currentlyInstantiatedEntities;
        private readonly AIEntityFactory _entityFactory;

        private readonly int _maxAIEntityType;
        private readonly float _newEntitySpawnRate;
        private int _maxEntitiesOnPlayground;

        public AIController(AIControllerConfiguration controllerConfiguration, AIEntityFactory entityFactory)
        {
            _maxEntitiesOnPlayground = controllerConfiguration.StartMaxEntitiesOnPlayGround;
            _newEntitySpawnRate = controllerConfiguration.SecsToAddEntity;

            _currentlyInstantiatedEntities = new HashSet<IAIEntity>();
            _maxAIEntityType = Enum.GetValues(typeof(AIMovingType)).Cast<int>().Max();

            _entityFactory = entityFactory;
            _entityFactory.SetEntitiesLists(controllerConfiguration.RandomMovingEntity, controllerConfiguration.ChasingArmoredEntity);
        }

        public IEnumerator Start(Vector2 playerStartPosition)
        {
            yield return SpawnEntities();
        }

        public void ChaseEnemy(Vector2 enemyPosition)
        {
            foreach (var entity in _currentlyInstantiatedEntities)
            {
                if (entity.MovingType == AIMovingType.Chasing)
                {
                    entity.SetMovementDirection(enemyPosition);
                }
            }
        }

        private void CreateEntity()
        {
            AIMovingType aiEntityTypeToCreate = (AIMovingType) Random.Range(0, _maxAIEntityType + 1);

            IAIEntity createdEntity = _entityFactory.CreateEntity(aiEntityTypeToCreate);
            
            if (createdEntity != null)
            {
                _currentlyInstantiatedEntities.Add(createdEntity);
                createdEntity.Run();
            }

            if (createdEntity is IDestroyableEntity destroyableEntity)
            {
                destroyableEntity.OnDestroyEntity += () =>
                {
                    OnEntityDestroyed(createdEntity);
                };
            }
        }

        private IEnumerator SpawnEntities()
        {
            while (true)
            {
                yield return new WaitUntil(() => _currentlyInstantiatedEntities.Count < _maxEntitiesOnPlayground);
                
                CreateEntity();
                
                yield return new WaitForSeconds(_newEntitySpawnRate);
            }
        }

        private void OnEntityDestroyed(IAIEntity destroyedEntity)
        {
            if (_currentlyInstantiatedEntities.Contains(destroyedEntity))
                _currentlyInstantiatedEntities.Remove(destroyedEntity);
        }
    }
}