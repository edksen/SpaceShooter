using AIModule.Configuration;
using SpaceShooter.PlayableObjects;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SpaceShooter.GameCore
{
    public class GameCore : MonoBehaviour
    {
        [SerializeField] private SpaceShip _shipPrefab;
        [SerializeField] private PlayerInput _inputSystem;
        [SerializeField] private AIControllerConfiguration _aiControllerConfiguration;
        [SerializeField] private PlayerStatusHandler _playerStatusHandler;

        private SpaceShooterInitializer _shooterInitializer;

        private void Awake()
        {
            _shooterInitializer = new SpaceShooterInitializer(_shipPrefab, _inputSystem, _aiControllerConfiguration, _playerStatusHandler);
        }

        private void Start()
        {
            _shooterInitializer.InitializeModules();
            _shooterInitializer.StartGame();
        }

        private void OnApplicationQuit()
        {
            _shooterInitializer.StopGame();
        }
    }
}