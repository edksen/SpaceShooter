using MovementSystem;
using MovementSystem.Contracts;
using SpaceShooter.AIModule;
using SpaceShooter.AIModule.Configuration;
using SpaceShooter.AIModule.Contracts;
using SpaceShooter.AIModule.Entity;
using SpaceShooter.ArmorSystem;
using SpaceShooter.ArmorSystem.Contracts;
using SpaceShooter.Controllers;
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
            
            StartCoroutine(_aiController.Start(player.Transform.position));
        }

        private void InitializeModules()
        {
            _borderController = new BorderController();
            var aiEntityFactory = new AIEntityFactory(_objectPool, _borderController);
            _aiController = new AIController(_aiControllerConfiguration, aiEntityFactory);
        }

        private SpaceShip CreatePlayer()
        {
            SpaceShip playerEntity = Instantiate(_shipPrefab);
            InertialEntityMovementController movementController = new InertialEntityMovementController(playerEntity, _borderController);

            movementController.OnPositionChanged += _aiController.ChaseEnemy;

            IArmoryController armoryController = new EntityArmorController(playerEntity, playerEntity.Armors);
            _playerInputController = new PlayerInputController(movementController, armoryController, _inputSystem);

            return playerEntity;
        }
    }
}