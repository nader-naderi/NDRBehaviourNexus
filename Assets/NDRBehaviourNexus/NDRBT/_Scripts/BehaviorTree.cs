using UnityEngine;

namespace NDRBT
{
    public abstract class BehaviorTree : MonoBehaviour
    {
        private Node root = null;

        protected void Start()
        {
            root = InitializeTree();
        }

        protected void Update()
        {
            if (root != null)
                root.Evaluate();
        }

        protected abstract Node InitializeTree();
    }
}