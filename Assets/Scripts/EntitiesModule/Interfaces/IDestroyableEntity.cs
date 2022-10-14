using System;

namespace TestShooter.Entities
{
    public interface IDestroyableEntity
    {
        event Action OnDestroyEntity;
    }
}