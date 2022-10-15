using System.Linq;
using SpaceShooter.ArmorSystem.Contracts;
using SpaceShooter.Entities;

namespace SpaceShooter.ArmorSystem
{
    public class EntityArmorController : IArmoryController
    {
        private IArmoredEntity _armoredEntity;

        public EntityArmorController(IArmoredEntity armoredEntity)
        {
            _armoredEntity = armoredEntity;

            if (_armoredEntity is IDestroyableEntity destroyableEntity)
                destroyableEntity.OnDestroyEntity += OnEntityDestroy;
        }
        
        public void MakeShot(ArmorType armorType)
        {
            GetArmor(armorType)?.MakeShot();
        }

        private Armor GetArmor(ArmorType armorType)
        {
            return _armoredEntity.Armors.FirstOrDefault(armor => armor.ArmorType == armorType);
        }

        private void OnEntityDestroy()
        {
            _armoredEntity = null;
        }
    }
}