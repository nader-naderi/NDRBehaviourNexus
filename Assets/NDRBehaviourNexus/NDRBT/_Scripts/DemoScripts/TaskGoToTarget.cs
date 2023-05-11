using UnityEngine;

namespace NDRBT
{
    public class TaskGoToTarget : Node
    {
        private Transform _transform;

        public TaskGoToTarget(Transform transform)
        {
            _transform = transform;
        }
        public override ENodeState Evaluate()
        {
            Transform target = (Transform)GetData("target");

            if (Vector3.Distance(_transform.position, target.position) > 0.01f)
            {
                _transform.position = Vector3.MoveTowards(_transform.position, target.position, 2f * Time.deltaTime);
                _transform.LookAt(target.position);
            }

            state = ENodeState.RUNNING;
            return state;
        }
    }
}
