using System.Collections.Generic;
using TestShooter.GameCore.Armor;

namespace TestShooter.Entities
{
    public interface IArmoredEntity
    {
        List<Armor> Armors { get; }
    }
}