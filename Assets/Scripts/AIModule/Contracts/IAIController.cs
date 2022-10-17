using System.Collections;
using UnityEngine;

namespace SpaceShooter.AIModule.Contracts
{
    public interface IAIController
    {
        void ChaseEnemy(Vector2 enemyPosition);
        public void Start(Vector2 playerStartPosition);

        public void ControllerUpdate();
    }
}