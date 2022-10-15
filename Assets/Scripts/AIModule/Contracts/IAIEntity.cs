using SpaceShooter.ArmorSystem.Contracts;
using SpaceShooter.MovingSystem.Contracts;

namespace SpaceShooter.AIModule.Contracts
{
    public interface IAIEntity
    {
        AIMovingType MovingType { get; }
        IMovementController MovementController { get; }
        IArmoryController ArmoryController { get; }
    }
}