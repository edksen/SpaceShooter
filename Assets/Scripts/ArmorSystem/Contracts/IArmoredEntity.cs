using System.Collections.Generic;
using System.Diagnostics;
using SpaceShooter.ArmorSystem.Settings;
using UnityEngine;

namespace SpaceShooter.ArmorSystem.Contracts
{
    public interface IArmoredEntity
    {
        List<ArmorConfiguration> Armors { get; }
        
        Transform ArmorTransform { get; }
    }
}