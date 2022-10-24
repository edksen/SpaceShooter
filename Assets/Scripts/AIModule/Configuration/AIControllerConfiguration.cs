using System.Collections.Generic;
using SpaceShooter.PlayableObjects;
using UnityEngine;

namespace AIModule.Configuration
{
    [CreateAssetMenu(fileName = "AIControllerConfiguration", menuName = "AI Controller Configuration", order = 0)]
    public class AIControllerConfiguration : ScriptableObject
    {
        public List<SpaceShip> ChasingArmoredEntity;
        public List<Asteroid> RandomMovingEntity;
        public int StartMaxEntitiesOnPlayGround;
        public int SecsToAddEntity;
        public int EntitiesAmountIncreaseRate;
    }
}