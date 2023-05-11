using UnityEngine;

namespace NDRBT
{
    public class TaskAttack : Node
    {
        Transform lastTarget;
        private float attackTimer = 1f;
        private float attackCoutner = 0;

        public override ENodeState Evaluate()
        {
            Transform target = (Transform)GetData("target");
            if (target != lastTarget)
            {
                lastTarget = target;
            }

            attackCoutner += Time.deltaTime;

            if (attackCoutner >= attackTimer)
            {
                bool enemyIsDead = false;
                if (enemyIsDead)
                {
                    ClearData("target");
                }
                else
                {
                    attackTimer = 0;
                }
            }

            state = ENodeState.RUNNING;
            return state;
        }
    }
}
