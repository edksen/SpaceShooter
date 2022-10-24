using System;
using UnityEngine;

namespace Entities
{
    public interface IDestructibleEntity
    {
        event Action<GameObject> OnDestroyCaughtEntity;
    }
}