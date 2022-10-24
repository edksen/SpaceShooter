using ArmorSystem.Armors;
using Entities;
using SpaceShooter.PlayableObjects;
using UnityEngine;

namespace SpaceShooter.GameCore
{
    public class Player
    {
        private PlayerController _controller;
        private PlayerStatusHandler _playerStatusHandler;
        private SpaceShip _playerEntity;

        public Player(SpaceShip playerEntity, 
            PlayerController playerController, 
            PlayerStatusHandler playerStatusHandler, 
            PlaygroundObjectObserver objectObserver)
        {
            _controller = playerController;
            _playerStatusHandler = playerStatusHandler;
            
            _controller.SetOnPositionChangeCallback(_playerStatusHandler.UpdateMovementInfo);
            _controller.SetOnArmorStatusChangeAction(ArmorType.Laser, _playerStatusHandler.UpdateLaserInfo);

            _playerEntity = playerEntity;
            
            objectObserver.SetOnDestroyAction(_playerEntity.gameObject, OnPlayerStop);
        }

        private void OnPlayerStop()
        {
            Object.Destroy(_playerEntity.gameObject);
            _controller.OnPlayerDestroyed();
            _controller = null;
            _playerEntity = null;
            _playerStatusHandler = null;
        }
    }
}