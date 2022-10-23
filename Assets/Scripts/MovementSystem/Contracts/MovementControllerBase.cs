using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace MovementSystem.Contracts
{
    public abstract class MovementControllerBase
    {
        public abstract event Action<Vector2> OnPositionChanged;
        public virtual float CurrentSpeed => _movableEntity.MaxSpeed;
        public float EntityRotation => _movableEntity.Transform.rotation.eulerAngles.z;

        protected IMovableEntity _movableEntity;
        protected IBorderController _borderController;
        protected Vector3 _movementDirection;
        private EntityMovingState _state;
        private CancellationTokenSource _tokenSource;

        protected MovementControllerBase(IMovableEntity movableEntity, IBorderController borderController)
        {
            _movableEntity = movableEntity;
            _state = EntityMovingState.Idle;
            _borderController = borderController;

            _movementDirection = Vector2.zero;
        }

        public virtual void MoveEntity(Vector2 direction)
        {
            _movementDirection = direction;
            if (TryChangeState(EntityMovingState.Moving))
            {
                _tokenSource = new CancellationTokenSource();
                StartMoving(_tokenSource.Token);
            }
        }

        protected abstract Task StartMoving(CancellationToken token);
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
        
        public void OnEntityDestroyed()
        {
            _tokenSource?.Cancel();
            _movableEntity = null;
            _borderController = null;
            _tokenSource?.Dispose();
        }
    }
}