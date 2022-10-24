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
        
        private readonly int _maxEntitiesOnPlaygroundCache;
        private readonly int _entitiesAmountIncreaseRate;
        private readonly int _maxAIEntityType;
        private readonly int _newEntitySpawnRate;
        
        private int _maxEntitiesOnPlayground;
        private int _secondsUntilNewEntity;

        private CancellationTokenSource _tokenSource;
        
        private PlaygroundObjectObserver _playgroundObjectObserver;

        public AIController(AIControllerConfiguration controllerConfiguration, AIEntityFactory entityFactory, PlaygroundObjectObserver objectObserver)
        {
            _maxEntitiesOnPlaygroundCache = controllerConfiguration.StartMaxEntitiesOnPlayGround;
            _newEntitySpawnRate = controllerConfiguration.SecsToAddEntity;
            _entitiesAmountIncreaseRate = controllerConfiguration.EntitiesAmountIncreaseRate;

            _currentlyInstantiatedEntities = new HashSet<IAIEntity>();
            _maxAIEntityType = Enum.GetValues(typeof(AIMovingType)).Cast<int>().Max();

            _entityFactory = entityFactory;
            _entityFactory.SetEntitiesLists(controllerConfiguration.RandomMovingEntity, controllerConfiguration.ChasingArmoredEntity);

            _playgroundObjectObserver = objectObserver;
        }

        public void Start(Vector2 playerStartPosition)
        {
            _maxEntitiesOnPlayground = _maxEntitiesOnPlaygroundCache;
            _tokenSource = new CancellationTokenSource();
            SpawnEntities(_tokenSource.Token);
            IncreaseEntitiesCount(_tokenSource.Token);
        }

        public void StopAIController()
        {
            _tokenSource?.Cancel();
            _tokenSource?.Dispose();
            _tokenSource = null;

            while (_currentlyInstantiatedEntities.Count > 0)
            {
                _playgroundObjectObserver.DestroyEntity(_currentlyInstantiatedEntities.First().EntityObject);
            }
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
                _secondsUntilNewEntity = _newEntitySpawnRate;
                while (_secondsUntilNewEntity > 0)
                {
                    await Task.Delay(1000, token);
                    --_secondsUntilNewEntity;
                }
            }
        }

        private async Task IncreaseEntitiesCount(CancellationToken token)
        {
            await Task.Delay(_entitiesAmountIncreaseRate * 1000, token);
            ++_maxEntitiesOnPlayground;
        }

        private void WaitWhenCanCreate(CancellationToken token)
        {
            while (_currentlyInstantiatedEntities.Count >= _maxEntitiesOnPlayground && !token.IsCancellationRequested) { }
        }

        private void CreateEntity()
        {
            AIMovingType aiEntityTypeToCreate = (AIMovingType) Random.Range(0, _maxAIEntityType + 1);

            IAIEntity createdEntity = _entityFactory.CreateEntity(aiEntityTypeToCreate);
            
            if (createdEntity != null)
            {
                _playgroundObjectObserver.SetOnDestroyAction(createdEntity.EntityObject, () =>
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
            _secondsUntilNewEntity = 0;
        }
    }
}