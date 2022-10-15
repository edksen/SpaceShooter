using System.Collections.Generic;

namespace SpaceShooter.ArmorSystem.Contracts
{
    public interface IArmoredEntity
    {
        List<Armor> Armors { get; }

        void SetArmor(List<Armor> armors);
    }
}