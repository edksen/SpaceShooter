using AIModule.Configuration;
using SpaceShooter.GameCore.UI;
using SpaceShooter.PlayableObjects;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SpaceShooter.GameCore
{
    public class Initializer : MonoBehaviour
    {
        [SerializeField] private SpaceShip _shipPrefab;
        [SerializeField] private PlayerInput _inputSystem;
        [SerializeField] private AIControllerConfiguration _aiControllerConfiguration;
        [SerializeField] private PlayerStatusHandler _playerStatusHandler;
        [SerializeField] private MainMenu _mainMenu;

        private SpaceShooterGameCore _shooterGameCore;

        private void Awake()
        {
            _shooterGameCore = new SpaceShooterGameCore(_shipPrefab, _inputSystem, _aiControllerConfiguration, _playerStatusHandler);
        }

        private void Start()
        {
            _shooterGameCore.InitializeModules(_mainMenu);
        }

        private void OnApplicationQuit()
        {
            _shooterGameCore.StopGame();
        }
    }
}