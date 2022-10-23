using AIModule;
using AIModule.Configuration;
using AIModule.Contracts;
using AIModule.Entity;
using ArmorSystem;
using ArmorSystem.Contracts;
using Entities;
using MovementSystem;
using MovementSystem.Contracts;
using SpaceShooter.PlayableObjects;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SpaceShooter.GameCore
{
    public class SpaceShooterInitializer
    {
        private SpaceShip _shipPrefab;
        private PlayerInput _inputSystem;
        private AIControllerConfiguration _aiControllerConfiguration;
        private PlayerStatusHandler _playerStatusHandler;

        private Player _player;
        private IBorderController _borderController;

        private IAIController _aiController;
        private SpaceShip _playerShip;
        
        private PlaygroundObjectObserver ObjectObserver => PlaygroundObjectObserver.Instance;

        public SpaceShooterInitializer(
            SpaceShip shipPrefab, 
            PlayerInput playerInput,
            AIControllerConfiguration aiControllerConfiguration,
            PlayerStatusHandler statusHandler)
        {
            _shipPrefab = shipPrefab;
            _inputSystem = playerInput;
            _aiControllerConfiguration = aiControllerConfiguration;
            _playerStatusHandler = statusHandler;
        }
        
        public void InitializeModules()
        {
            _borderController = new BorderController();
            var aiEntityFactory = new AIEntityFactory(_borderController);
            _aiController = new AIController(_aiControllerConfiguration, aiEntityFactory);
        }

        public void StartGame()
        {
            CreatePlayer();
            StartAI();
        }
        
        public void StopGame()
        {
            _player.DestroyPlayer();
            _player = null;
        }

        private void CreatePlayer()
        {
            _playerShip = Object.Instantiate(_shipPrefab);
            InertialEntityMovementController movementController = new InertialEntityMovementController(_playerShip, _borderController);

            movementController.OnPositionChanged += _aiController.ChaseEnemy;

            IArmoryController armoryController = new EntityArmorController(_playerShip, _playerShip.Armors);
            var playerController = new PlayerController(movementController, armoryController, _inputSystem);

            _player = new Player(_playerShip, playerController, _playerStatusHandler);
            
            ObjectObserver.SetOnDestroyAction(_playerShip.gameObject, StopGame);
        }

        private void StartAI()
        {
            _aiController.Start(_playerShip.Transform.position);
        }
    }
}