using System.Collections.Generic;

namespace NDRBT
{
    public class Sequence : Node
    {
        public Sequence() : base() { }
        public Sequence(List<Node> children) : base(children) { }

        public override ENodeState Evaluate()
        {
            bool isAnyChildRunning = false;

            foreach (Node node in children)
            {
                switch (node.Evaluate())
                {
                    case ENodeState.RUNNING:
                        isAnyChildRunning = true;
                        continue;
                    case ENodeState.SUCCESS:
                        continue;
                    case ENodeState.FAILURE:
                        state = ENodeState.FAILURE;
                        return state;
                    default:
                        state = ENodeState.SUCCESS;
                        return state;
                }
            }

            state = isAnyChildRunning ? ENodeState.RUNNING : ENodeState.SUCCESS;
            return state;
        }
    }
}