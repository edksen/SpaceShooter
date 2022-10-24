using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Entities
{
    public class PlaygroundObjectObserver
    {
        private readonly Dictionary<GameObject, Action> _entitiesOnPlayground;

        public PlaygroundObjectObserver()
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
                Action onDestroy = _entitiesOnPlayground[gameObject];
                _entitiesOnPlayground.Remove(gameObject);
                onDestroy?.Invoke();
            }
        }

        public void DestroyAllEntities()
        {
            while (_entitiesOnPlayground.Count > 0)
            {
                GameObject go = _entitiesOnPlayground.First().Key;
                DestroyEntity(go);
            }
        }
    }
}