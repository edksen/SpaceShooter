using System;
using System.Threading;
using System.Threading.Tasks;
using MovementSystem.Contracts;
using UnityEngine;

namespace MovementSystem
{
    public class RegularEntityMovementController : MovementControllerBase
    {
        public override event Action<Vector2> OnPositionChanged;
        public RegularEntityMovementController(IMovableEntity movableEntity, IBorderController borderController) : base(movableEntity, borderController)
        {
        }

        protected override async Task StartMoving(CancellationToken token)
        {
            do
            {
                await Task.Delay((int) (Time.deltaTime * 1000), token);
                CalculateNewPosition();
                _borderController.CheckEntity(_movableEntity, _movementDirection);
                OnPositionChanged?.Invoke(_movableEntity.Transform.position);

            } while (_movementDirection != Vector3.zero&& !token.IsCancellationRequested);

            TryChangeState(EntityMovingState.Idle);
        }

        protected override void CalculateNewPosition()
        {
            _movableEntity.Transform.Translate(_movableEntity.MaxSpeed * _movementDirection);
        }
    }
}