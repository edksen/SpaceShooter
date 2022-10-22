using System;
using MovementSystem.Contracts;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MovementSystem
{
    public class BorderController : IBorderController
    {
        private const float EPSILON = 0.5f;
        private readonly Vector2 _stageDimensions;

        public BorderController()
        {
            _stageDimensions = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        }
        
        public void CheckEntity(IMovableEntity entity, Vector2 currentDirection)
        {
            if (InBorders(entity))
                return;

            switch (entity.BorderState)
            {
                case EntityBorderState.Destroy:
                    DestroyEntity(entity);
                    break;
                case EntityBorderState.Move:
                    MoveEntityToAnotherBorder(entity.Transform, currentDirection);
                    break;
                case EntityBorderState.Change:
                    ChangeEntityDirection(entity);
                    break;
                default:
                    break;
            }
        }

        public Vector2 GetRandomPointInBorder()
        {
            var randomPoint = Random.insideUnitCircle;
            return randomPoint * _stageDimensions;
        }

        private bool InBorders(IMovableEntity movableEntity)
        {
            var entityPosition = movableEntity.Transform.position;
            return Math.Abs(entityPosition.x) <= _stageDimensions.x + EPSILON &&
                   Math.Abs(entityPosition.y) <= _stageDimensions.y + EPSILON;
        }

        private void ChangeEntityDirection(IMovableEntity entity)
        {
            const float minRotationAngle = 20;
            const float maxRotationAngle = 90;

            float randomAngle = Random.Range(minRotationAngle, maxRotationAngle);
            entity.Transform.Rotate(0f, 0f, 180 - randomAngle);
        }

        private void MoveEntityToAnotherBorder(Transform entityTransform, Vector2 currentDirection)
        {
            var inverseDirection = CalculateInverseDirection(currentDirection, entityTransform.rotation); 
            entityTransform.position = inverseDirection.normalized * _stageDimensions;
        }

        private void DestroyEntity(IMovableEntity entity)
        {
            GameObject.Destroy(entity.Transform.gameObject);
        }

        private Vector2 CalculateInverseDirection(Vector2 currentDirection, Quaternion entityRotation)
        {
            const float inverseAngle = 180f;
            
            return Quaternion.Euler(0, 0, inverseAngle) * (entityRotation * currentDirection);
        }
    }
}