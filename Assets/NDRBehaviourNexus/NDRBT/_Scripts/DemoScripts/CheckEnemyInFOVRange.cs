using UnityEngine;

namespace NDRBT
{
    public class CheckEnemyInFOVRange : Node
    {
        private Transform _transform;
        public CheckEnemyInFOVRange(Transform transform)
        {
            this._transform = transform;
        }
        public override ENodeState Evaluate()
        {

            object t = GetData("target");
            if (t == null)
            {
                Collider[] colliders = Physics.OverlapSphere(_transform.position, 25, 1 << 6);
                if (colliders.Length > 0)
                {
                    Parent.Parent.SetData("target", colliders[0].transform);

                    state = ENodeState.SUCCESS;
                    return state;
                }
            }


            state = ENodeState.SUCCESS;
            return state;
        }
    }
}
