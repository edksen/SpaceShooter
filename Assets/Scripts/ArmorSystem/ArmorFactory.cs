using System.ComponentModel;
using ArmorSystem.Armors;
using ArmorSystem.Contracts;
using ArmorSystem.Settings;
using Entities;
using UnityEngine;

namespace ArmorSystem
{
    public class ArmorFactory
    {
        private PlaygroundObjectObserver _objectObserver;

        public ArmorFactory(PlaygroundObjectObserver objectObserver)
        {
            _objectObserver = objectObserver;
            
        }
        public Armor CreateArmor(ArmorConfiguration armorConfiguration, Transform armoryTransform)
        {
            switch (armorConfiguration.ArmorType)
            {
                case ArmorType.Bomb:
                    return new BombArmor(armorConfiguration.Projectile, armoryTransform, armorConfiguration.AttackRate, armorConfiguration.AmmoCapacity, _objectObserver);
                case ArmorType.Bullet:
                    return new LightArmor(armorConfiguration.Projectile, armoryTransform, _objectObserver);
                case ArmorType.Laser:
                    return new LaserArmor(armorConfiguration.Projectile, armoryTransform, armorConfiguration.AmmoCapacity, armorConfiguration.RechargeRateInSecs, _objectObserver);
                default:
                    throw new InvalidEnumArgumentException(
                        $"[ArmoryFactory] Cannot create armor with type : {armorConfiguration.ArmorType}");
            }
        }
    }
}