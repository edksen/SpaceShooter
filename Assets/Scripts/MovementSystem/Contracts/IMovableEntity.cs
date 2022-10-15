using SpaceShooter.Controllers;
using UnityEngine;

namespace SpaceShooter.Entities
{
    public interface IMovableEntity
    {
        float GasForce { get; }
        float MaxSpeed { get; }
        float Inertia { get; }
        float RotationSpeed { get; }
        Transform Transform { get; }
        EntityBorderState BorderState { get; }
        MonoBehaviour CoroutineRunner { get; }
    }
}