using System.Collections.Generic;

using UnityEngine;

namespace NDRBT
{
    public class PawnBehaviour : BehaviorTree
    {
        [SerializeField] private new Transform[] waypoints;

        protected override Node InitializeTree()
        {
            Node root = new Selector(new List<Node>()
            {
                new Sequence(new List<Node>()
                {
                    new CheckEnemyInAttackRange(transform),
                    new TaskAttack(),
                }),
                new Sequence(new List<Node>()
                {
                    new CheckEnemyInFOVRange(transform),
                    new TaskGoToTarget(transform),
                }),
            });

            return root;
        }
    }
}
