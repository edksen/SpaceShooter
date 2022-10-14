using System;
using System.Collections;
using Controllers.EntityControllers;
using TestShooter.Controllers;
using TestShooter.Entities;
using TestShooter.GameCore.Factories;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TestShooter.GameCore
{
    public class GameCoreInitializer : MonoBehaviour, IInstanceCreator
    {
        [SerializeField] private SpaceShip _shipPrefab;
        [SerializeField] private PlayerInput _inputSystem;
        
        private PlayerInputController _playerInputController;
        private IEntityControllerFactory _entityEntityControllerFactory;
        private IEntityFactory _entityFactory;
        private IBorderController _borderController;

        public T InstantiateObject<T>(T objectToInstantiate, Transform referenceTransform) where T : MonoBehaviour
        {
            return Instantiate(objectToInstantiate, referenceTransform);
        }

        private void Start()
        {
            InitializeModules();
            CreatePlayer();
        }

        private void InitializeModules()
        {
            _borderController = new BorderController();
            _entityEntityControllerFactory = new EntityControllerFactory(_borderController);
            _entityFactory = new EntityFactory(this, _entityEntityControllerFactory);
        }

        private void CreatePlayer()
        {
            _entityFactory.CreateArmoredMovingEntity(_shipPrefab, null, out var shipMovementController, out var shipArmoryController);
            _playerInputController = new PlayerInputController(shipMovementController, shipArmoryController, _inputSystem);
        }

        private void OnApplicationQuit()
        {
            StopAllCoroutines();
        }
    }
}