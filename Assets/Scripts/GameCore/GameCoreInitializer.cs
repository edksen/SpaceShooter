using Controllers.EntityControllers;
using SpaceShooter.ArmorSystem;
using SpaceShooter.ArmorSystem.Contracts;
using SpaceShooter.Controllers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SpaceShooter.GameCore
{
    public class GameCoreInitializer : MonoBehaviour
    {
        [SerializeField] private SpaceShip _shipPrefab;
        [SerializeField] private PlayerInput _inputSystem;
        
        private PlayerInputController _playerInputController;
        private IBorderController _borderController;

        private void Start()
        {
            InitializeModules();
            CreatePlayer();
        }

        private void InitializeModules()
        {
            _borderController = new BorderController();
        }

        private void CreatePlayer()
        {
            SpaceShip playerEntity = Instantiate(_shipPrefab);
            IMovementController movementController = new EntityMovementController(playerEntity, _borderController);
            IArmoryController armoryController = new EntityArmorController(playerEntity);
            _playerInputController = new PlayerInputController(movementController, armoryController, _inputSystem);
        }
    }
}