using System;
using UnityEngine;

namespace Entities
{
    public interface IDestroyableEntity
    {
        event Action<GameObject> OnDestroyCaughtEntity;
    }
}