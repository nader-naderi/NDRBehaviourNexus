using NDRBT;

using System.Collections.Generic;

using UnityEngine;

namespace NDRBehaviourNexus
{
    [CreateAssetMenu]
    public class BehaviourGraph : ScriptableObject
    {
        public List<SavedStateNode> savedWrapperNodes = new List<SavedStateNode>();
        Dictionary<StateNode, SavedStateNode> stateNodeSavedWrapperMap =
            new Dictionary<StateNode, SavedStateNode>();

        Dictionary<State, StateNode> stateDict = new Dictionary<State, StateNode>();

        public void Init()
        {
            stateNodeSavedWrapperMap.Clear();
            stateDict.Clear();
        }


        public void SetNode(BaseNode node)
        {
            if (node is StateNode stateNode)
                if (!stateNode.isDuplicate)
                    SetStateNode(stateNode);

            if (node is TransitionNode transitionNode)
            {
                SetTransitionNode(transitionNode);
            }

            if (node is CommentNode commentNode)
            {
                SetCommentNode(commentNode);
            }

        }
        public void SetCommentNode(CommentNode node)
        {

        }

        public StateNode GetDuplicateStateNode(StateNode node)
        {
            foreach (State state in stateDict.Keys)
                if (state == node.CurrentState)
                    return stateDict[state];

            return null;
        }

        public bool IsStateNodeDuplicate(StateNode node) => GetDuplicateStateNode(node);

        private void SetStateNode(StateNode node)
        {
            if (node.PreviousState != null)
                stateDict.Remove(node.PreviousState);

            if (node.CurrentState == null)
                return;

            SavedStateNode savedNode = null;

            foreach (var wrapper in savedWrapperNodes)
            {
                if (wrapper.state == node.CurrentState)
                {
                    savedNode = wrapper;
                    break;
                }
            }

            if (savedNode == null)
            {
                savedNode = new SavedStateNode();
                savedWrapperNodes.Add(savedNode);
            }

            if (!stateNodeSavedWrapperMap.ContainsKey(node))
                stateNodeSavedWrapperMap.Add(node, savedNode);

            savedNode.state = node.CurrentState;
            savedNode.position = new Vector2(node.windowRect.x, node.windowRect.y);
            savedNode.isCollapsed = node.IsCollapsed;

            if (!stateDict.ContainsKey(node.CurrentState))
                stateDict.Add(node.CurrentState, node);
        }

        public void ClearStateNode(StateNode node)
        {
            SavedStateNode s = GetSavedState(node);

            if (s != null)
            {
                savedWrapperNodes.Remove(s);
                stateNodeSavedWrapperMap.Remove(node);
            }
        }

        private SavedStateNode GetSavedState(StateNode node)
        {
            stateNodeSavedWrapperMap.TryGetValue(node, out SavedStateNode savedNode);
            return savedNode;
        }

        private StateNode GetStateNode(State node)
        {
            stateDict.TryGetValue(node, out StateNode r);
            return r;
        }
        public bool IsTransitionDuplicate(TransitionNode node)
            => GetSavedState(node.enteringState).IsTransitionDuplicate(node);

        private void SetTransitionNode(TransitionNode transitionNode)
        {
           
            SavedStateNode savedState = GetSavedState(transitionNode.enteringState);
            savedState.SetTransitionNode(transitionNode);
        }
    }

    [System.Serializable]
    public class SavedStateNode
    {
        public State state;
        public Vector2 position;
        public bool isCollapsed;

        public List<SavedConditionsNode> savedCondition = new List<SavedConditionsNode>();

        Dictionary<TransitionNode, SavedConditionsNode> savedTransDict =
            new Dictionary<TransitionNode, SavedConditionsNode>();

        Dictionary<Condition, TransitionNode> conditionsDict = 
            new Dictionary<Condition, TransitionNode>();

        public void Init()
        {
            savedTransDict.Clear();
            conditionsDict.Clear();
        }

        public bool IsTransitionDuplicate(TransitionNode node)
        {
            bool retValue = false;
            conditionsDict.TryGetValue(node.targetCondition, out TransitionNode prevNode);

            if (prevNode != null)
                retValue = true;

            return retValue;
        }

        public void SetTransitionNode(TransitionNode node)
        {
            if (node.isDuplicate)
                return;

            if (node.previousTransition != null)
                conditionsDict.Remove(node.targetCondition);

            if (node.targetCondition == null)
                return;

            SavedConditionsNode condition = GetSavedConditionNode(node);
            if (condition == null)
            {
                condition = new SavedConditionsNode();
                savedCondition.Add(condition);
                savedTransDict.Add(node, condition);
                node.transition = node.enteringState.CurrentState.AddTransition();
            }

            condition.transition = node.transition;
            condition.condition = node.targetCondition;
            condition.transition.Condition = condition.condition;
            condition.position = new Vector2(node.windowRect.x, node.windowRect.y);
            
            conditionsDict.Add(condition.condition, node);
        }

        private SavedConditionsNode GetSavedConditionNode(TransitionNode node)
        {
            savedTransDict.TryGetValue(node, out SavedConditionsNode savedNode);
            return savedNode;
        }

        public TransitionNode GetStateNode(Transition node)
        {
            conditionsDict.TryGetValue(node.Condition, out TransitionNode transition);
            return transition;
        }
    }

    [System.Serializable]
    public class SavedConditionsNode
    {
        public Transition transition;
        public Condition condition;
        public Vector2 position;
    }
}
