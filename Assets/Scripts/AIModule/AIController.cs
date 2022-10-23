using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AIModule.Configuration;
using AIModule.Contracts;
using AIModule.Entity;
using Entities;
using UnityEngine;
using Object = UnityEngine.Object;
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

        private CancellationTokenSource _tokenSource;
        
        private PlaygroundObjectObserver PlaygroundObjectObserver => PlaygroundObjectObserver.Instance;

        public AIController(AIControllerConfiguration controllerConfiguration, AIEntityFactory entityFactory)
        {
            _maxEntitiesOnPlayground = controllerConfiguration.StartMaxEntitiesOnPlayGround;
            _newEntitySpawnRate = controllerConfiguration.SecsToAddEntity;

            _currentlyInstantiatedEntities = new HashSet<IAIEntity>();
            _maxAIEntityType = Enum.GetValues(typeof(AIMovingType)).Cast<int>().Max();

            _entityFactory = entityFactory;
            _entityFactory.SetEntitiesLists(controllerConfiguration.RandomMovingEntity, controllerConfiguration.ChasingArmoredEntity);
        }

        public void Start(Vector2 playerStartPosition)
        {
            _tokenSource = new CancellationTokenSource();
            SpawnEntities(_tokenSource.Token);
        }

        public void StopAIController()
        {
            _tokenSource?.Cancel();
            _tokenSource?.Dispose();
            _currentlyInstantiatedEntities.Clear();
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
        
        private async Task SpawnEntities(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                await Task.Run(() => WaitWhenCanCreate(token));
                CreateEntity();
                await Task.Delay((int)(_newEntitySpawnRate * 1000), token);
            }
        }

        private Task WaitWhenCanCreate(CancellationToken token)
        {
            while (_currentlyInstantiatedEntities.Count >= _maxEntitiesOnPlayground && !token.IsCancellationRequested) { }
            
            return Task.CompletedTask;
        }

        private void CreateEntity()
        {
            AIMovingType aiEntityTypeToCreate = (AIMovingType) Random.Range(0, _maxAIEntityType + 1);

            IAIEntity createdEntity = _entityFactory.CreateEntity(aiEntityTypeToCreate);
            
            if (createdEntity != null)
            {
                PlaygroundObjectObserver.SetOnDestroyAction(createdEntity.EntityObject, () =>
                {
                    Object.Destroy(createdEntity.EntityObject);
                    OnEntityDestroyed(createdEntity);
                });
                
                _currentlyInstantiatedEntities.Add(createdEntity);
                createdEntity.Run();
            }
        }

        private void OnEntityDestroyed(IAIEntity destroyedEntity)
        {
            if (_currentlyInstantiatedEntities.Contains(destroyedEntity))
                _currentlyInstantiatedEntities.Remove(destroyedEntity);
            
            destroyedEntity.OnAIEntityDestroy();
        }
    }
}