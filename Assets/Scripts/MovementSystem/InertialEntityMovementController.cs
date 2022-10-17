using System;
using System.Collections;
using MovementSystem.Contracts;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;

namespace MovementSystem
{
    public class InertialEntityMovementController : MovementControllerBase
    {
        public event Action<Vector2> OnPositionChanged;
        
        private Vector2 _currentDirection;
        private float _currentSpeed;

        public InertialEntityMovementController(IMovableEntity movableEntity, IBorderController borderController)
            : base(movableEntity, borderController)
        {
            _currentDirection = Vector2.zero;
            _currentSpeed = 0f;
        }

        protected override IEnumerator StartMoving()
        {
            do
            {
                yield return new WaitForFixedUpdate();
                _currentDirection += _movableEntity.Inertia * Time.fixedDeltaTime * (_movementDirection - _currentDirection);
                CalculateNewPosition();
                OnPositionChanged?.Invoke(_movableEntity.Transform.position);
                
            } while (_currentDirection != Vector2.zero);

            TryChangeState(EntityMovingState.Idle);
        }

        protected override void CalculateNewPosition()
        {
            Quaternion rotation = _movableEntity.Transform.rotation;
            float z = rotation.eulerAngles.z;
            z -= _currentDirection.x * _movableEntity.RotationSpeed * Time.deltaTime;
            rotation = Quaternion.Euler(0, 0, z);
            _movableEntity.Transform.rotation = rotation;

            _currentSpeed = Math.Abs(_currentDirection.y) * (_currentSpeed + _movableEntity.GasForce * Time.deltaTime);
            _currentSpeed = Math.Clamp(_currentSpeed, -_movableEntity.MaxSpeed,
                _movableEntity.MaxSpeed);
            Vector3 velocity = new Vector3(0, _currentSpeed * Math.Sign(_currentDirection.y), 0);
            _movableEntity.Transform.position += rotation * velocity;
                
            _borderController.CheckEntity(_movableEntity, _currentDirection);
        }
    }
}