using UnityEditor;

using UnityEngine;

namespace NDRBehaviourNexus
{
    public class TransitionNode : BaseNode
    {
        public bool isDuplicate;
        public Condition targetCondition;
        public Condition previousCondition;
        public Transition transition;
        public Transition previousTransition;
        public StateNode enteringState;
        public StateNode exitingState;

        public void Init(StateNode enteringState, Transition targetTransition)
        {
            this.enteringState = enteringState;
        }

        public override void DrawWindow()
        {
            EditorGUILayout.LabelField("");

            targetCondition = (Condition)EditorGUILayout.ObjectField(targetCondition,
                typeof(Condition), false);

            if (targetCondition == null)
            {
                EditorGUILayout.LabelField("No condition");
            }
            else
            {
                if (isDuplicate)
                {
                    EditorGUILayout.LabelField("Duplicate Condition!");
                }
                else
                {
                    //if (transition == null)
                    //    return;

                    //transition.IsDisbale = EditorGUILayout.Toggle("Disable", transition.IsDisbale);
                }

            }

            if (previousCondition != targetCondition)
            {
                isDuplicate = BehaviourEditor.currentGraph.IsTransitionDuplicate(this);

                if (!isDuplicate)
                    BehaviourEditor.currentGraph.SetNode(this);

                previousCondition = targetCondition;
            }
        }

        public override void DrawCurve()
        {
            if (enteringState)
            {
                Rect rect = windowRect;
                rect.y += windowRect.height * .5f;
                rect.width = 1;
                rect.height = 1;

                BehaviourEditor.DrawNodeCurve(enteringState.windowRect, rect, true, Color.black);
            }
        }
    }
}