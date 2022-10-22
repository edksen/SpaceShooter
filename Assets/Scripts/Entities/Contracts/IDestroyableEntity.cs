using System;

namespace Entities
{
    public interface IDestroyableEntity
    {
        event Action OnDestroyEntity;
    }
}