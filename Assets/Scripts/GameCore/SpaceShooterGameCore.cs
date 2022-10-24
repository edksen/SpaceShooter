using AIModule;
using AIModule.Configuration;
using AIModule.Contracts;
using AIModule.Entity;
using ArmorSystem;
using ArmorSystem.Contracts;
using Entities;
using MovementSystem;
using MovementSystem.Contracts;
using SpaceShooter.GameCore.UI;
using SpaceShooter.PlayableObjects;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SpaceShooter.GameCore
{
    public class SpaceShooterGameCore
    {
        private SpaceShip _shipPrefab;
        private PlayerInput _inputSystem;
        private AIControllerConfiguration _aiControllerConfiguration;
        private PlayerStatusHandler _playerStatusHandler;

        private Player _player;
        private Score _playerScore;
        private IBorderController _borderController;

        private IAIController _aiController;
        private SpaceShip _playerShip;

        private MainMenuHandler _mainMenuHandler;
        
        private PlaygroundObjectObserver ObjectObserver => PlaygroundObjectObserver.Instance;

        public SpaceShooterGameCore(
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
        
        public void InitializeModules(MainMenu mainMenu)
        {
            _playerScore = new Score();

            _borderController = new BorderController();
            
            _aiController = new AIController(_aiControllerConfiguration, new AIEntityFactory(_borderController));
            
            InitMainMenu(mainMenu);
        }

        public void StopGame()
        {
            if(_playerShip)
                ObjectObserver.DestroyEntity(_playerShip.gameObject);
        }

        private void InitMainMenu(MainMenu mainMenu)
        {
            _mainMenuHandler = new MainMenuHandler(mainMenu);
            
            _mainMenuHandler.InitWindow(_playerScore.GetHighScore(), StartGame, ExitGame);
        }

        private void StartGame()
        {
            _mainMenuHandler.HideMainMenu();
            _playerStatusHandler.UpdateScore(_playerScore.CurrentScore);
            _playerStatusHandler.gameObject.SetActive(true);
            CreatePlayer();
            StartAI();
        }

        private void CreatePlayer()
        {
            _playerShip = Object.Instantiate(_shipPrefab);
            _playerShip.OnDestroyCaughtEntity += ObjectObserver.DestroyEntity;
            
            InertialEntityMovementController movementController = new InertialEntityMovementController(_playerShip, _borderController);
            movementController.OnPositionChanged += _aiController.ChaseEnemy;
            
            IArmoryController armoryController = new EntityArmorController(_playerShip, _playerShip.Armors);
            armoryController.SetOnProjectileHitAction(UpdateScore);
            
            var playerController = new PlayerController(movementController, armoryController, _inputSystem);

            _player = new Player(_playerShip, playerController, _playerStatusHandler);

            ObjectObserver.SetOnDestroyAction(_playerShip.gameObject, OnGameStop);
        }
        
        private void OnGameStop()
        {
            _mainMenuHandler.UpdateScore(_playerScore.CurrentScore);
            _playerScore.ResetScore();
            _aiController.StopAIController();
            ObjectObserver.DestroyAllEntities();
            _playerStatusHandler.gameObject.SetActive(false);
            _mainMenuHandler.ShowMainMenu();
        }

        private void ExitGame()
        {
            _playerScore.SaveScore();
            Application.Quit();
        }

        private void StartAI()
        {
            _aiController.Start(_playerShip.Transform.position);
        }

        private void UpdateScore()
        {
            _playerScore.IncreaseScore();
            _playerStatusHandler.UpdateScore(_playerScore.CurrentScore);
        }
    }
}