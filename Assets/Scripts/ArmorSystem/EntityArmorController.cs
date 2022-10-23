using System;
using System.Collections.Generic;
using ArmorSystem.Armors;
using ArmorSystem.Contracts;
using ArmorSystem.Settings;

namespace ArmorSystem
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
        }
        
        public void MakeShot(ArmorType armorType)
        {
            GetArmor(armorType)?.MakeShot();
        }

        private Armor GetArmor(ArmorType armorType)
        {
            return _entityArmors.Find(armor => armor.ArmorType == armorType);
        }

        public void OnEntityDestroyed()
        {
            foreach (var armor in _entityArmors)
            {
                armor.OnArmorDestroy();
            }
            
            _entityArmors.Clear();
            _entityArmors = null;
            _armoredEntity = null;
        }

        public void SetArmorInfoCallback(ArmorType armorType, Action<int, float> callback)
        {
            GetArmor(armorType).SetOnArmorStatusUpdated(callback);
        }
    }
}