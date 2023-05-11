using UnityEngine;

namespace NDRBT
{
    public class CheckEnemyInAttackRange : Node
    {
        private Transform _transform;
        public CheckEnemyInAttackRange(Transform transform)
        {
            this._transform = transform;
        }
        public override ENodeState Evaluate()
        {
            object t = GetData("target");
            
            if (t == null)
            {
                state = ENodeState.FAILURE;
                return state;
            }

            Transform target = (Transform)t;

            if (Vector3.Distance(_transform.position, target.position) <= 10f)
            {
                // Attack
                state = ENodeState.SUCCESS;
                return state;
            }

            state = ENodeState.SUCCESS;
            return state;
        }
    }
}
