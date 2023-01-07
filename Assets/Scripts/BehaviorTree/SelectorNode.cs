using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public class SelectorNode : Node
    {
        public SelectorNode() : base() { }
        public SelectorNode(List<Node> children) : base(children) { }

        public override ENodeState Evaluate()
        {
            foreach (Node node in children)
            {
                switch (node.Evaluate())
                {
                    case ENodeState.FAILURE:
                        continue;

                    case ENodeState.SUCCESS:
                        nodeState = ENodeState.SUCCESS;
                        return nodeState;

                    case ENodeState.RUNNING:
                        nodeState = ENodeState.RUNNING;
                        return nodeState;

                    default:
                        continue;
                }
            }

            nodeState = ENodeState.FAILURE;
            return nodeState;
        }
    }
}