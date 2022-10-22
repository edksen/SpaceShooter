using System;
using System.Collections;
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

        protected override IEnumerator StartMoving()
        {
            do
            {
                yield return new WaitForEndOfFrame();
                CalculateNewPosition();
                _borderController.CheckEntity(_movableEntity, _movementDirection);
                OnPositionChanged?.Invoke(_movableEntity.Transform.position);

            } while (_movementDirection != Vector3.zero);

            TryChangeState(EntityMovingState.Idle);
        }

        protected override void CalculateNewPosition()
        {
            _movableEntity.Transform.Translate(_movableEntity.MaxSpeed * _movementDirection);
        }
    }
}