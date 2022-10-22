using System;
using System.Collections;
using Entities;
using UnityEngine;

namespace MovementSystem.Contracts
{
    public abstract class MovementControllerBase
    {
        public abstract event Action<Vector2> OnPositionChanged;
        
        protected IMovableEntity _movableEntity;
        protected IBorderController _borderController;
        protected Vector3 _movementDirection;
        
        private Coroutine _movingCoroutine;
        private EntityMovingState _state;

        protected MovementControllerBase(IMovableEntity movableEntity, IBorderController borderController)
        {
            _movableEntity = movableEntity;
            _state = EntityMovingState.Idle;
            _borderController = borderController;

            _movementDirection = Vector2.zero;

            if (_movableEntity is IDestroyableEntity destroyableEntity)
                destroyableEntity.OnDestroyEntity += OnEntityDestroy;
        }

        public void MoveEntity(Vector2 direction)
        {
            _movementDirection = direction;
            if (_state == EntityMovingState.Idle && TryChangeState(EntityMovingState.Moving))
            {
                _movingCoroutine = _movableEntity.CoroutineRunner
                    .StartCoroutine(StartMoving());
            }
        }

        protected abstract IEnumerator StartMoving();
        protected abstract void CalculateNewPosition();

        protected bool TryChangeState(EntityMovingState newState)
        {
            switch (newState)
            {
                case EntityMovingState.Idle when _state == EntityMovingState.Moving:
                case EntityMovingState.Moving when _state == EntityMovingState.Idle:
                    _state = newState;
                    return true;
                default:
                    return false;
            }
        }
        
        private void OnEntityDestroy()
        {
            if (_movingCoroutine != null)
            {
                _movableEntity.CoroutineRunner.StopAllCoroutines();
                _movingCoroutine = null;
            }
            _movableEntity = null;
            _borderController = null;
        }
    }
}