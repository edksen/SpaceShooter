using System;
using System.Collections;
using MovementSystem.Contracts;
using Unity.VisualScripting;
using UnityEngine;

namespace MovementSystem
{
    public class RegularEntityMovementController : MovementControllerBase
    {
        public event Action<Vector2> OnEntityMove;
        public RegularEntityMovementController(IMovableEntity movableEntity, IBorderController borderController) : base(movableEntity, borderController)
        {
        }

        protected override IEnumerator StartMoving()
        {
            do
            {
                yield return new WaitForNextFrameUnit();
                CalculateNewPosition();
                _borderController.CheckEntity(_movableEntity, _movementDirection);
                OnEntityMove?.Invoke(_movableEntity.Transform.position);

            } while (_movementDirection != Vector2.zero);

            TryChangeState(EntityMovingState.Idle);
        }

        protected override void CalculateNewPosition()
        {
            _movableEntity.Transform.Translate(_movableEntity.MaxSpeed * _movementDirection);
            if (_movableEntity.RotationSpeed != 0)
            {
                _movableEntity.Transform.rotation = Quaternion.Slerp(
                    _movableEntity.Transform.rotation, 
                    Quaternion.Euler(0,0, _movableEntity.RotationSpeed * Time.deltaTime), 
                    1);
            }
        }
    }
}