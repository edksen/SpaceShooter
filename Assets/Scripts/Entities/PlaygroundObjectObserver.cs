using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Entities
{
    public class PlaygroundObjectObserver
    {
        public static PlaygroundObjectObserver Instance => _instance ??= new PlaygroundObjectObserver();
        private static PlaygroundObjectObserver _instance;

        private readonly Dictionary<GameObject, Action> _entitiesOnPlayground;

        private PlaygroundObjectObserver()
        {
            _entitiesOnPlayground = new Dictionary<GameObject, Action>();
        }

        public void SetOnDestroyAction(GameObject gameObject, Action onDestroyAction)
        {
            if (!_entitiesOnPlayground.ContainsKey(gameObject))
            {
                _entitiesOnPlayground.Add(gameObject, onDestroyAction);
            }
            else
            {
                _entitiesOnPlayground[gameObject] += onDestroyAction;
            }
        }

        public void DestroyEntity(GameObject gameObject)
        {
            if (_entitiesOnPlayground.ContainsKey(gameObject))
            {
                _entitiesOnPlayground[gameObject]?.Invoke();
                _entitiesOnPlayground.Remove(gameObject);
            }
        }
    }
}