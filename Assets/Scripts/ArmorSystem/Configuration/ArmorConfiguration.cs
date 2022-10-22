using ArmorSystem.Armors;
using UnityEngine;

namespace ArmorSystem.Settings
{
    [CreateAssetMenu(fileName = "ArmorConfiguration", menuName = "Armor Configuration Asset", order = 0)]
    public class ArmorConfiguration : ScriptableObject
    {
        public ArmorType ArmorType;
        public Projectile Projectile;
        public int AttackRate;
        public float RechargeRateInSecs;
        public int AmmoCapacity;
    }
}