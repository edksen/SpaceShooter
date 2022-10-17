using System;
using Controllers.EntityControllers;
using SpaceShooter.AIModule;
using SpaceShooter.AIModule.Configuration;
using SpaceShooter.AIModule.Contracts;
using SpaceShooter.ArmorSystem;
using SpaceShooter.ArmorSystem.Contracts;
using SpaceShooter.Controllers;
using SpaceShooter.MovingSystem;
using SpaceShooter.MovingSystem.Contracts;
using SpaceShooter.PlayableObjects;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SpaceShooter.GameCore
{
    public class GameCoreInitializer : MonoBehaviour
    {
        [SerializeField] private SpaceShip _shipPrefab;
        [SerializeField] private PlayerInput _inputSystem;
        [SerializeField] private Transform _objectPool;
        [SerializeField] private AIControllerConfiguration _aiControllerConfiguration;
        
        private PlayerInputController _playerInputController;
        private IBorderController _borderController;

        private IAIController _aiController;

        private void Start()
        {
            InitializeModules();
            var player = CreatePlayer();
            
            _aiController.Start(player.Transform.position);
        }

        private void Update()
        {
            _aiController.ControllerUpdate();
        }

        private void InitializeModules()
        {
            _borderController = new BorderController();
            _aiController = new AIController(_borderController, _aiControllerConfiguration, _objectPool);
        }

        private SpaceShip CreatePlayer()
        {
            SpaceShip playerEntity = Instantiate(_shipPrefab);
            IMovementController movementController = new EntityMovementController(playerEntity, _borderController);

            movementController.OnPositionChanged += _aiController.ChaseEnemy;

            IArmoryController armoryController = new EntityArmorController(playerEntity, playerEntity.Armors);
            _playerInputController = new PlayerInputController(movementController, armoryController, _inputSystem);

            return playerEntity;
        }
    }
}