using TestShooter.GameCore.Armor;

namespace Controllers.EntityControllers
{
    public interface IArmoryController
    {
        void MakeShot(ArmorType armorType, bool directional = true);
    }
}