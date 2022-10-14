using UnityEngine;

namespace TestShooter.GameCore
{
    public interface IInstanceCreator
    {
        T InstantiateObject<T>(T objectToInstantiate, Transform referenceTransform) where T : MonoBehaviour;
    }
}