using System;
using System.Collections;
using Controllers.EntityControllers;
using TestShooter.Entities;
using Unity.VisualScripting;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;

namespace TestShooter.Controllers
{
    public class EntityMovementController : IMovementController
    {
        private IBorderController _borderController;
        private IMovableEntity _movableEntity;
        
        private EntityMovingState _state;
        private Vector2 _movementDirection;
        private Vector2 _currentDirection;
        private float _currentSpeed;

        private Coroutine _movingCoroutine;

        public EntityMovementController(IMovableEntity ship, IBorderController borderController)
        {
            _movableEntity = ship;
            _state = EntityMovingState.Idle;
            _borderController = borderController;

            _movementDirection = Vector2.zero;
            _currentDirection = Vector2.zero;
            _currentSpeed = 0f;

            if (_movableEntity is IDestroyableEntity destroyableEntity)
                destroyableEntity.OnDestroyEntity += OnEntityDestroy;
        }
        
        public void MoveEntity(Vector2 direction, bool withoutInertia = false)
        {
            _movementDirection = direction;
            if (_state == EntityMovingState.Idle && TryChangeState(EntityMovingState.Moving))
            {
                _movingCoroutine = _movableEntity.CoroutineRunner
                    .StartCoroutine(withoutInertia ? MoveEntityWithoutInertia() : MoveEntityWithInertia());
            }
        }

        private bool TryChangeState(EntityMovingState newState)
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

        private IEnumerator MoveEntityWithInertia()
        {
            do
            {
                yield return new WaitForFixedUpdate();
                _currentDirection += _movableEntity.Inertia * Time.fixedDeltaTime * (_movementDirection - _currentDirection);
                CalculateNewPosition();
                
            } while (_currentDirection != Vector2.zero);

            TryChangeState(EntityMovingState.Idle);
        }

        private IEnumerator MoveEntityWithoutInertia()
        {
            do
            {
                yield return new WaitForNextFrameUnit();
                _currentDirection += _movementDirection - _currentDirection;
                CalculateNewPosition();
                
            } while (_currentDirection != Vector2.zero);

            TryChangeState(EntityMovingState.Idle);
        }

        private void CalculateNewPosition()
        {
            Quaternion rotation = _movableEntity.Transform.rotation;
            float z = rotation.eulerAngles.z;
            z -= _currentDirection.x * _movableEntity.RotationSpeed * Time.deltaTime;
            rotation = Quaternion.Euler(0, 0, z);
            _movableEntity.Transform.rotation = Quaternion.Slerp(rotation, rotation, 1);

            _currentSpeed = Math.Abs(_currentDirection.y) * (_currentSpeed + _movableEntity.GasForce * Time.deltaTime);
            _currentSpeed = Math.Clamp(_currentSpeed, -_movableEntity.MaxSpeed,
                _movableEntity.MaxSpeed);
            Vector3 velocity = new Vector3(0, _currentSpeed * Math.Sign(_currentDirection.y), 0);
            _movableEntity.Transform.position += rotation * velocity;
                
            _borderController.CheckEntity(_movableEntity, _currentDirection);
        }

        private void OnEntityDestroy()
        {
            if (_movingCoroutine != null)
            {
                _movableEntity.CoroutineRunner.StopCoroutine(_movingCoroutine);
                _movingCoroutine = null;
            }
            _movableEntity = null;
            _borderController = null;
        }
    }
}