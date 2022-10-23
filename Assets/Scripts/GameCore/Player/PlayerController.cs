using System;
using ArmorSystem.Armors;
using MovementSystem.Contracts;
using ArmorSystem.Contracts;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SpaceShooter.GameCore
{
    public class PlayerController
    {
        private MovementControllerBase _shipMovementController;
        private IArmoryController _shipArmoryController;
        private PlayerInput _playerInput;

        public PlayerController(
            MovementControllerBase shipMovementController, 
            IArmoryController shipArmoryController, 
            PlayerInput playerInput)
        {
            _shipMovementController = shipMovementController;
            _shipArmoryController = shipArmoryController;

            _playerInput = playerInput;
            _playerInput.onActionTriggered += OnActionTriggered;
        }

        public void SetOnPositionChangeCallback(Action<Vector2, float, float> onPositionChanged)
        {
            _shipMovementController.OnPositionChanged += pos => onPositionChanged(pos, _shipMovementController.EntityRotation, _shipMovementController.CurrentSpeed);
        }

        public void SetOnArmorStatusChangeAction(ArmorType armorType, Action<int, float> onArmorStatusChanged)
        {
            _shipArmoryController.SetArmorInfoCallback(armorType, onArmorStatusChanged);
        }

        public void OnPlayerDestroyed()
        {
            _shipArmoryController.OnEntityDestroyed();
            _shipMovementController.OnEntityDestroyed();
            _shipArmoryController = null;
            _shipMovementController = null;
            _playerInput.onActionTriggered -= OnActionTriggered;
            _playerInput = null;
        }
        
        private void OnActionTriggered(InputAction.CallbackContext context)
        {
            string actionName = context.action.name;
            switch (actionName)
            {
                case "Fire":
                    OnFire(context);
                    break;
                case "Laser":
                    OnLaser(context);
                    break;
                default:
                    OnMove(context);
                    break;
            }
            
        }

        private void OnMove(InputAction.CallbackContext context)
        {
            _shipMovementController.MoveEntity(context.ReadValue<Vector2>());
        }

        private void OnFire(InputAction.CallbackContext context)
        {
            if(context.phase == InputActionPhase.Started)
                _shipArmoryController.MakeShot(ArmorType.Bullet);
        }

        private void OnLaser(InputAction.CallbackContext context)
        {
            if(context.phase == InputActionPhase.Started)
                _shipArmoryController.MakeShot(ArmorType.Laser);
        }
    }
}