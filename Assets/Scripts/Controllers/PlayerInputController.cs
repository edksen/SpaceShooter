using Controllers.EntityControllers;
using TestShooter.GameCore.Armor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TestShooter.Controllers
{
    public class PlayerInputController
    {
        private IMovementController _shipMovementController;
        private IArmoryController _shipArmoryController;

        public PlayerInputController(
            IMovementController shipMovementController, 
            IArmoryController shipArmoryController, 
            PlayerInput playerInput)
        {
            _shipMovementController = shipMovementController;
            _shipArmoryController = shipArmoryController;
            playerInput.onActionTriggered += OnActionTriggered;
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
                _shipArmoryController.MakeShot(ArmorType.Light);
        }

        private void OnLaser(InputAction.CallbackContext context)
        {
            if(context.phase == InputActionPhase.Started)
                _shipArmoryController.MakeShot(ArmorType.Laser);
        }
    }
}