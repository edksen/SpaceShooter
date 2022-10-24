using System;
using System.Threading;
using System.Threading.Tasks;
using MovementSystem.Contracts;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;

namespace MovementSystem
{
    public class InertialEntityMovementController : MovementControllerBase
    {
        private const int FIXED_UPDATE_TIME = 8;
        public override event Action<Vector2> OnPositionChanged;
        public override float CurrentSpeed => _currentSpeed;

        private Vector3 _currentDirection;
        private float _currentSpeed;

        public InertialEntityMovementController(IMovableEntity movableEntity, IBorderController borderController)
            : base(movableEntity, borderController)
        {
            _currentDirection = Vector2.zero;
            _currentSpeed = 0f;
        }

        public override void MoveEntity(Vector2 direction)
        {
            var entityDirection = new Vector2(direction.x, Mathf.Clamp(direction.y, 0, 1));
            base.MoveEntity(entityDirection);
        }

        protected override async Task StartMoving(CancellationToken token)
        {
            do
            {
                await Task.Delay(FIXED_UPDATE_TIME, token);
                _currentDirection += (1 - _movableEntity.Inertia) * Time.fixedDeltaTime * (_movementDirection - _currentDirection);
                CalculateNewPosition();
                OnPositionChanged?.Invoke(_movableEntity.Transform.position);
                
            } while (_currentDirection != Vector3.zero && !token.IsCancellationRequested);

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
            _currentSpeed = Math.Clamp(_currentSpeed, -_movableEntity.MaxSpeed, _movableEntity.MaxSpeed);
            Vector3 rotationVelocity = new Vector3(0, _currentSpeed * Math.Sign(_currentDirection.y), 0);
            _movableEntity.Transform.position += rotation * rotationVelocity;
                
            _borderController.CheckEntity(_movableEntity, _currentDirection);
        }
    }
}