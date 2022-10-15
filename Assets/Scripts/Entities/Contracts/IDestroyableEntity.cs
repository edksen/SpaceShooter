using System;

namespace SpaceShooter.Entities
{
    public interface IDestroyableEntity
    {
        event Action OnDestroyEntity;
    }
}