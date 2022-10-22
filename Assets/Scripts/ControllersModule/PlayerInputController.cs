using ArmorSystem.Armors;
using MovementSystem.Contracts;
using ArmorSystem.Contracts;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Controllers
{
    public class PlayerInputController
    {
        private MovementControllerBase _shipMovementController;
        private IArmoryController _shipArmoryController;

        public PlayerInputController(
            MovementControllerBase shipMovementController, 
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
                _shipArmoryController.MakeShot(ArmorType.Bullet);
        }

        private void OnLaser(InputAction.CallbackContext context)
        {
            if(context.phase == InputActionPhase.Started)
                _shipArmoryController.MakeShot(ArmorType.Static);
        }
    }
}