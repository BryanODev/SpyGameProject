using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public class SequenceNode : Node
    {
        public SequenceNode() : base() { }
        public SequenceNode(List<Node> children) : base(children) { }

        public override ENodeState Evaluate()
        {
            bool anyChildIsRunning = false;

            foreach (Node node in children) 
            {
                switch (node.Evaluate()) 
                {
                    case ENodeState.FAILURE:
                        nodeState = ENodeState.FAILURE;
                        return nodeState;

                    case ENodeState.SUCCESS:
                        continue;

                    case ENodeState.RUNNING:
                        anyChildIsRunning = true;
                        return nodeState;

                    default:
                        nodeState = ENodeState.SUCCESS;
                        return nodeState;
                }
            }

            nodeState = anyChildIsRunning ? ENodeState.RUNNING : ENodeState.SUCCESS;
            return nodeState;
        }
    }
}