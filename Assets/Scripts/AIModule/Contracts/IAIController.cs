using System.Collections;
using UnityEngine;

namespace SpaceShooter.AIModule.Contracts
{
    public interface IAIController
    {
        void ChaseEnemy(Vector2 enemyPosition);
        public IEnumerator Start(Vector2 playerStartPosition);
    }
}