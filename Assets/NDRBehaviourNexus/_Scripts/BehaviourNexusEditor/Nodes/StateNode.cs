using System.Collections.Generic;

using UnityEditor;

using UnityEditorInternal;

using UnityEngine;

namespace NDRBehaviourNexus
{
    public class StateNode : BaseNode
    {
        private State currentState;
        private State previousState;
        private bool isCollapsed;
        private bool previousCollapsedState;
        public bool isDuplicate = false;

        private List<BaseNode> dependencies = new List<BaseNode>();

        SerializedObject serializedState;

        ReorderableList onStateList;
        ReorderableList onEnterList;
        ReorderableList onExitList;

        public List<BaseNode> Dependencies { get => dependencies; }
        public bool IsCollapsed { get => isCollapsed; set => isCollapsed = value; }
        public State PreviousState { get => previousState; }
        public State CurrentState { get => currentState; set => currentState = value; }

        public override void DrawWindow()
        {
            if (currentState == null)
                EditorGUILayout.LabelField("Add State to modify: ");
            else
            {
                windowRect.height = 100;

                isCollapsed = EditorGUILayout.Toggle(" ", isCollapsed);
            }


            currentState = (State)EditorGUILayout.ObjectField(currentState, typeof(State), false);

            if (previousCollapsedState != isCollapsed)
            {
                previousCollapsedState = isCollapsed;
                BehaviourEditor.currentGraph.SetNode(this);
            }

            if (previousState != currentState)
            {
                serializedState = null;

                isDuplicate = BehaviourEditor.currentGraph.IsStateNodeDuplicate(this);

                if (!isDuplicate)
                {
                    BehaviourEditor.currentGraph.SetNode(this);
                    previousState = currentState;

                    for (int i = 0; i < currentState.transitions.Count; i++)
                    {

                    }
                }
            }

            if (isDuplicate)
            {
                EditorGUILayout.LabelField("State is a duplicate");
                windowRect.height = 100;
                return;
            }

            if (currentState != null)
            {
                if (serializedState == null)
                {
                    serializedState = new SerializedObject(currentState);
                    onStateList = new ReorderableList(serializedState, serializedState.FindProperty("onState"), true, true, true, true);
                    onEnterList = new ReorderableList(serializedState, serializedState.FindProperty("onEnter"), true, true, true, true);
                    onExitList = new ReorderableList(serializedState, serializedState.FindProperty("onExit"), true, true, true, true);
                }
                if (isCollapsed)
                    return;

                serializedState.Update();
                HandleReorderableList(onStateList, "On State");
                HandleReorderableList(onEnterList, "On Enter");
                HandleReorderableList(onExitList, "On Exit");


                EditorGUILayout.LabelField("");
                onStateList.DoLayoutList();

                EditorGUILayout.LabelField("");
                onEnterList.DoLayoutList();

                EditorGUILayout.LabelField("");
                onExitList.DoLayoutList();

                serializedState.ApplyModifiedProperties();

                float standard = 300;

                standard += (onStateList.count) * 20;

                windowRect.height = standard;
            }
        }

        void HandleReorderableList(ReorderableList list, string targetName)
        {
            list.drawHeaderCallback = (Rect rect) =>
            {
                EditorGUI.LabelField(rect, targetName);
            };

            list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                var element = list.serializedProperty.GetArrayElementAtIndex(index);
                EditorGUI.ObjectField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), element, GUIContent.none);
            };
        }

        public override void DrawCurve()
        {

        }

        public Transition AddTransition()
        {
            return currentState.AddTransition();
        }

        public void ClearReferences()
        {
            BehaviourEditor.ClearWindowsFromList(dependencies);
            dependencies.Clear();
        }
    }
}