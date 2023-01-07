using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public enum ENodeState 
    {
        RUNNING,
        FAILURE,
        SUCCESS
    }

    public class Node
    {

        protected ENodeState nodeState;

        public Node parentNode;
        protected List<Node> children = new List<Node>();

        public Node()
        {
            parentNode = null;
        }

        public Node(List<Node> children)
        {
            foreach (Node child in children)
            {
                _Attach(child);
            }
        }

        private void _Attach(Node node)
        {
            node.parentNode = this;
            children.Add(node);
        }

        public virtual ENodeState Evaluate() => ENodeState.FAILURE;

        private Dictionary<string, object> _dataContext = new Dictionary<string, object>();

        public void SetData(string key, object value)
        {
            _dataContext[key] = value;
        }

        public object GetData(string key)
        {
            object value = null;

            if (_dataContext.TryGetValue(key, out value))
            {
                return value;
            }

            Node node = parentNode;

            while (node != null)
            {
                value = node.GetData(key);

                if (value != null)
                {
                    return value;
                }

                node = node.parentNode;
            }

            return null;
        }

        public bool ClearData(string key)
        {
            if (_dataContext.ContainsKey(key))
            {
                _dataContext.Remove(key);
                return true;
            }

            Node node = parentNode;

            while (node != null)
            {
                bool cleared = node.ClearData(key);

                if (cleared)
                {
                    return true;
                }

                node = node.parentNode;
            }

            return false;
        }
    }
}
