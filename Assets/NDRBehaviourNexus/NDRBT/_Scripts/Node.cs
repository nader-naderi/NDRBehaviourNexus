using System.Collections.Generic;

namespace NDRBT
{
    public enum ENodeState
    {
        RUNNING,
        SUCCESS,
        FAILURE
    }

    public class Node
    {
        protected ENodeState state;
        public Node Parent { get; set; }
        protected List<Node> children;

        public Dictionary<string, object> dataContext
         = new Dictionary<string, object>();

        public Node()
        {
            Parent = null;
        }

        public Node(List<Node> children)
        {
            foreach (Node child in children)
                Attach(child);
        }

        public void Attach(Node node)
        {
            node .Parent = this;
            children.Add(node);
        }

        public virtual ENodeState Evaluate() => ENodeState.FAILURE;

        public void SetData(string key, object value)
        {
            dataContext[key] = value;
        }

        public object GetData(string key)
        {
            object value = null;

            if (dataContext.TryGetValue(key, out value))
                return value;

            Node node = Parent;

            while (node != null)
            {
                value = node.GetData(key);
                if(value != null)
                    return value;

                node = node.Parent;
            }

            return null;
        }

        public bool ClearData(string key)
        {
            if (dataContext.ContainsKey(key))
                return true;

            Node node = Parent;

            while (node != null)
            {
                bool cleared = node.ClearData(key);
                if (cleared)
                    return true;

                node = node.Parent;
            }

            return false;
        }
    }
}