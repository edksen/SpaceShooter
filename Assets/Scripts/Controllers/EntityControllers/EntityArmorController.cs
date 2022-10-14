using System.Linq;
using TestShooter.Entities;
using TestShooter.GameCore.Armor;
using TestShooter.GameCore.Factories;

namespace Controllers.EntityControllers
{
    public class EntityArmorController : IArmoryController
    {
        private IArmoredEntity _armoredEntity;
        private IEntityFactory _projectileControllerFactory;

        public EntityArmorController(IArmoredEntity armoredEntity, IEntityFactory projectileControllerFactory)
        {
            _projectileControllerFactory = projectileControllerFactory;
            _armoredEntity = armoredEntity;
            
            if (_armoredEntity is IDestroyableEntity destroyableEntity)
                destroyableEntity.OnDestroyEntity += OnEntityDestroy;
        }
        
        public void MakeShot(ArmorType armorType, bool directional = true)
        {
            GetArmor(armorType)?.MakeShot(_projectileControllerFactory, directional);
        }

        private Armor GetArmor(ArmorType armorType)
        {
            return _armoredEntity.Armors.FirstOrDefault(armor => armor.ArmorType == armorType);
        }

        private void OnEntityDestroy()
        {
            _armoredEntity = null;
            _projectileControllerFactory = null;
        }
    }
}