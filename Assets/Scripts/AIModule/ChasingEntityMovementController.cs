using System;
using System.Threading;
using System.Threading.Tasks;
using MovementSystem;
using MovementSystem.Contracts;
using UnityEngine;

namespace AIModule
{
    public class ChasingEntityMovementController : MovementControllerBase
    {
        private static readonly float RotationEpsilon = 0.5f;

        public bool CanMakeShot
        {
            get;
            private set;
        }

        public ChasingEntityMovementController(IMovableEntity movableEntity, IBorderController borderController) : base(movableEntity, borderController)
        {
        }

        public override event Action<Vector2> OnPositionChanged;
        protected override async Task StartMoving(CancellationToken token)
        {
            do
            {
                await Task.Delay((int) (Time.deltaTime * 1000), token);
                CalculateNewPosition();
                _borderController.CheckEntity(_movableEntity, _movementDirection);
                OnPositionChanged?.Invoke(_movableEntity.Transform.position);

            } while (_movementDirection != Vector3.zero && !token.IsCancellationRequested);

            TryChangeState(EntityMovingState.Idle);
        }

        protected override void CalculateNewPosition()
        {
            Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, _movementDirection);

            if (targetRotation == _movableEntity.Transform.rotation)
            {
                CanMakeShot = true;
            }
            else
            {
                CanMakeShot = false;
                _movableEntity.Transform.rotation = Quaternion.RotateTowards(_movableEntity.Transform.rotation, targetRotation,
                        _movableEntity.RotationSpeed * Time.deltaTime);
            }
            
            _movableEntity.Transform.Translate(_movableEntity.MaxSpeed * Time.deltaTime * Vector3.up, Space.Self);
        }
    }
}