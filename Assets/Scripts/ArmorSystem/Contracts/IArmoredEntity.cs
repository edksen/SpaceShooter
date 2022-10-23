using System.Collections.Generic;
using ArmorSystem.Settings;
using UnityEngine;

namespace ArmorSystem.Contracts
{
    public interface IArmoredEntity
    {
        List<ArmorConfiguration> Armors { get; }
        Transform ArmorTransform { get; }
    }
}