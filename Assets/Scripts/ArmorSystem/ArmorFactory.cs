using System.ComponentModel;
using SpaceShooter.ArmorSystem.Contracts;
using SpaceShooter.ArmorSystem.Settings;
using UnityEngine;

namespace SpaceShooter.ArmorSystem
{
    internal class ArmorFactory
    {
        private static ArmorFactory _instance;
        public static ArmorFactory Factory => _instance ??= new ArmorFactory();

        private ArmorFactory() {}

        public Armor CreateArmor(ArmorConfiguration armorConfiguration, Transform armoryTransform)
        {
            switch (armorConfiguration.ArmorType)
            {
                case ArmorType.Bomb:
                    return new BombArmor(armorConfiguration.Projectile, armoryTransform, armorConfiguration.AttackRate, armorConfiguration.AmmoCapacity);
                case ArmorType.Bullet:
                    return new LightArmor(armorConfiguration.Projectile, armoryTransform);
                case ArmorType.Static:
                    return new LaserArmor(armorConfiguration.Projectile, armoryTransform, armorConfiguration.AmmoCapacity, armorConfiguration.RechargeRateInSecs);
                default:
                    throw new InvalidEnumArgumentException(
                        $"[ArmoryFactory] Cannot create armor with type : {armorConfiguration.ArmorType}");
            }
        }
    }
}