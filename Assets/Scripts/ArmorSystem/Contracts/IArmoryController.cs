using System;
using ArmorSystem.Armors;

namespace ArmorSystem.Contracts
{
    public interface IArmoryController
    {
        void MakeShot(ArmorType armorType);
        void OnEntityDestroyed();
        void SetArmorInfoCallback(ArmorType armorType, Action<int, float> callback);
        void SetOnProjectileHitAction(Action onProjectileHit);
    }
}