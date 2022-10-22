using System;
using System.Collections;
using MovementSystem;
using MovementSystem.Contracts;
using UnityEngine;

namespace SpaceShooter.AIModule
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