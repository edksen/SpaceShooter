using System.Collections.Generic;
using SpaceShooter.ArmorSystem.Contracts;
using SpaceShooter.ArmorSystem.Settings;
using SpaceShooter.Entities;

namespace SpaceShooter.ArmorSystem
{
    public class EntityArmorController : IArmoryController
    {
        private IArmoredEntity _armoredEntity;
        private List<Armor> _entityArmors;

        public EntityArmorController(IArmoredEntity armoredEntity, List<ArmorConfiguration> armorConfigurations)
        {
            _armoredEntity = armoredEntity;
            _entityArmors = new List<Armor>(armorConfigurations.Count);

            foreach (var armor in armorConfigurations)
            {
                _entityArmors.Add(ArmorFactory.Factory.CreateArmor(armor, _armoredEntity.ArmorTransform));
            }

            if (_armoredEntity is IDestroyableEntity destroyableEntity)
                destroyableEntity.OnDestroyEntity += OnEntityDestroy;
        }
        
        public void MakeShot(ArmorType armorType)
        {
            GetArmor(armorType)?.MakeShot();
        }

        private Armor GetArmor(ArmorType armorType)
        {
            return _entityArmors.Find(armor => armor.ArmorType == armorType);
        }

        private void OnEntityDestroy()
        {
            _armoredEntity = null;
        }
    }
}