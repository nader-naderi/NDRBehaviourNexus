using System;
using System.Collections.Generic;

using UnityEditor;

using UnityEngine;

namespace NDRBehaviourNexus
{
    public class BehaviourEditor : EditorWindow
    {
        /// <summary>
        /// List of user actions.
        /// </summary>
        public enum UserAction { AddStateNode, MakeTransitionNode, DeleteNode, AddCommentNode }

        /// <summary>
        /// all of the nodes
        /// </summary>
        private static List<BaseNode> windows = new List<BaseNode>();

        private Vector3 mousePosition;


        private bool isCreatingTransition;

        private bool isClickedOnWindow;

        private BaseNode currentSelectedNode;

        private int currentSelectedNodeIndex;

        public static BehaviourGraph currentGraph;
        static GraphNode graphNode;

        [MenuItem("NDR Behvaiour Nexus/Editor")]
        private static void ShowEditor()
        {
            BehaviourEditor editor = GetWindow<BehaviourEditor>();
            editor.minSize = new Vector3(800, 600);
        }

        private void OnGUI()
        {
            Event m_Event = Event.current;
            mousePosition = m_Event.mousePosition;
            UserInput(m_Event);
            DrawWindows();
        }

        private void OnEnable()
        {
            if (graphNode == null)
            {
                graphNode = CreateInstance<GraphNode>();
                graphNode.windowRect = new Rect(10, position.height * .7f, 200, 100);
                graphNode.windowTitle = "Graph";
            }

            windows.Clear();

            windows.Add(graphNode);
            LoadGraph();
        }

        private void DrawWindows()
        {
            BeginWindows();

            DrawCurves();

            DrawNodes();

            EndWindows();
        }

        private void DrawNodes()
        {
            for (int i = 0; i < windows.Count; i++)
                windows[i].windowRect = GUI.Window(i, windows[i].windowRect, DrawNodeWindow, windows[i].windowTitle);
        }

        private static void DrawCurves()
        {
            foreach (BaseNode node in windows)
                node.DrawCurve();
        }

        private void DrawNodeWindow(int id)
        {
            windows[id].DrawWindow();
            GUI.DragWindow();
        }

        private void UserInput(Event m_Event)
        {
            if (m_Event.button == 1 && !isCreatingTransition)
                if (m_Event.type == EventType.MouseDown)
                    OnRightClickEventHandler(m_Event);

            if (m_Event.button == 0 && !isCreatingTransition)
            {
                if (m_Event.type == EventType.MouseDown)
                {
                    //OnLeftClickEventHandler(m_Event);
                }

                if (m_Event.type == EventType.MouseDrag)
                {
                    for (int i = 0; i < windows.Count; i++)
                    {
                        if (windows[i].windowRect.Contains(m_Event.mousePosition))
                        {
                            if (currentGraph != null)
                                currentGraph.SetNode(windows[i]);
                            
                            break;
                        }
                    }
                }

            }
        }

        private void OnRightClickEventHandler(Event m_Event)
        {
            currentSelectedNodeIndex = -1;
            isClickedOnWindow = false;

            for (int i = 0; i < windows.Count; i++)
            {
                if (windows[i].windowRect.Contains(m_Event.mousePosition))
                {
                    isClickedOnWindow = true;
                    currentSelectedNode = windows[i];
                    currentSelectedNodeIndex = i;
                    break;
                }
            }


            if (!isClickedOnWindow)
                AddNewNode(m_Event);
            else
                ModifyNodes(m_Event);
        }

        private void AddNewNode(Event e)
        {
            GenericMenu menu = new GenericMenu();

            menu.AddSeparator("");
            if (currentGraph != null)
            {
                menu.AddItem(new GUIContent("Add New State"), false, ContextCallback, UserAction.AddStateNode);
                menu.AddItem(new GUIContent("Add New Comment"), false, ContextCallback, UserAction.AddCommentNode);

            }
            else
            {
                menu.AddDisabledItem(new GUIContent("Add State"));
                menu.AddDisabledItem(new GUIContent("Add Comment"));
            }

            menu.ShowAsContext();
            e.Use();
        }

        private void ModifyNodes(Event e)
        {
            GenericMenu menu = new GenericMenu();

            if (currentSelectedNode is StateNode stateNode)
            {
                if (stateNode != null)
                {
                    menu.AddSeparator("");
                    menu.AddItem(new GUIContent("Make Transition"), false, ContextCallback, UserAction.MakeTransitionNode);
                }
                else
                {
                    menu.AddSeparator("");
                    menu.AddDisabledItem(new GUIContent("Add Transition"));
                }

                menu.AddSeparator("");
                menu.AddItem(new GUIContent("Delete"), false, ContextCallback, UserAction.DeleteNode);
            }

            if (currentSelectedNode is TransitionNode)
            {

                menu.AddSeparator("");
                menu.AddItem(new GUIContent("Delete"), false, ContextCallback, UserAction.DeleteNode);
            }

            if (currentSelectedNode is CommentNode)
            {

                menu.AddSeparator("");
                menu.AddItem(new GUIContent("Delete"), false, ContextCallback, UserAction.DeleteNode);
            }

            menu.ShowAsContext();
            e.Use();
        }

        private void ContextCallback(object obj)
        {
            UserAction userAction = (UserAction)obj;
            switch (userAction)
            {
                case UserAction.AddStateNode:
                    AddStateNodeAction();
                    break;
                case UserAction.MakeTransitionNode:
                    MakeTransitionNodeAction();
                    break;
                case UserAction.DeleteNode:
                    DeleteNodeAction();
                    break;
                case UserAction.AddCommentNode:
                    AddCommentNodeAction();
                    break;
                default:
                    break;
            }
        }

        private void AddStateNodeAction()
        {
            AddStateNode(mousePosition);
        }

        private void MakeTransitionNodeAction()
        {
            if (currentSelectedNode is StateNode fromNode)
            {
                //Transition transition = fromNode.AddTransition();
                AddTransitionNode(fromNode.CurrentState.transitions.Count,
                    null, fromNode);
            }
        }

        private void DeleteNodeAction()
        {
            if (currentSelectedNode is StateNode targetState)
            {
                targetState.ClearReferences();
                windows.Remove(targetState);
                currentGraph.ClearStateNode(targetState);
            }

            if (currentSelectedNode is TransitionNode targetTrans)
            {
                windows.Remove(targetTrans);

                //if (targetTrans.enteringState.CurrentState.transitions.Contains(targetTrans.targetCondition))
                //    targetTrans.enteringState.CurrentState.transitions.Remove(targetTrans.targetCondition);

                
            }

            if (currentSelectedNode is CommentNode)
            {
                windows.Remove(currentSelectedNode);
            }

            currentSelectedNode = null;
            isClickedOnWindow = false;
        }

        private void AddCommentNodeAction()
        {
            AddCommentNode(mousePosition);
        }


        #region HelperMethods

        public static StateNode AddStateNode(Vector2 pos)
        {
            StateNode newNode = CreateInstance<StateNode>();

            newNode.windowRect = new Rect(pos.x, pos.y, 200, 300);
            newNode.windowTitle = "New State";

            windows.Add(newNode);

            // currentGraph.SetStateNode(newNode);

            return newNode;
        }

        public static CommentNode AddCommentNode(Vector2 pos)
        {
            CommentNode newCommentNode = CreateInstance<CommentNode>();

            newCommentNode.windowRect = new Rect(pos.x, pos.y, 200, 100);

            newCommentNode.windowTitle = "New Comment";

            windows.Add(newCommentNode);

            return newCommentNode;
        }

        public static TransitionNode AddTransitionNode(int index, Transition transition, StateNode from)
        {
            Rect fromRect = from.windowRect;

            fromRect.x += 50;

            float targetY = fromRect.y - fromRect.height;

            if (from.CurrentState != null)
            {
                targetY = (index * 100);
            }

            fromRect.y = targetY;
            fromRect.x += 200 + 100;
            fromRect.y += (fromRect.height * .7f);

            Vector2 pos = new Vector2(fromRect.x, fromRect.y);

            return AddTransitionNode(pos, transition, from);
        }

        public static TransitionNode AddTransitionNode(Vector2 pos, Transition transition, StateNode from)
        {
            TransitionNode node = CreateInstance<TransitionNode>();
            node.Init(from, transition);
            node.windowRect = new Rect(pos.x, pos.y, 200, 80);
            node.windowTitle = "Condition Check";
            windows.Add(node);
            from.Dependencies.Add(node);
            return node;
        }

        public static void DrawNodeCurve(Rect start, Rect end, bool left, Color curveColor)
        {
            Vector3 startPos = new Vector3((left) ? start.x + start.width : start.x,
                start.y + (start.height * .5f),
                0);

            Vector3 endPos = new Vector3(end.x + (end.width + .5f), end.y + (end.height * .5f), 0);

            Vector3 startTan = startPos + Vector3.right * 50;
            Vector3 endTan = endPos + Vector3.left * 50;

            Color shadow = new Color(0, 0, 0, 0.06f);

            for (int i = 0; i < 3; i++)
                Handles.DrawBezier(startPos, endPos, startTan, endTan, shadow, null, (i * 1) * .5f);

            Handles.DrawBezier(startPos, endPos, startPos, endTan, curveColor, null, 1);
        }

        public static void ClearWindowsFromList(List<BaseNode> nodes)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                if (windows.Contains(nodes[i]))
                {
                    windows.Remove(nodes[i]);
                }
            }
        }

        public static void LoadGraph()
        {
            windows.Clear();
            windows.Add(graphNode);

            if (currentGraph == null)
                return;

            currentGraph.Init();

            LoadStateNodes(currentGraph.savedWrapperNodes);

            LoadTransitions();
        }

        private static void LoadStateNodes(IEnumerable<SavedStateNode> savedStateNodes)
        {
            var queue = new Queue<SavedStateNode>(savedStateNodes);
            currentGraph.savedWrapperNodes.Clear();

            while (queue.Count > 0)
            {
                var stateNodeWrapper = queue.Dequeue();
                var stateNode = AddStateNode(stateNodeWrapper.position);
                
                stateNode.CurrentState = stateNodeWrapper.state;
                stateNode.IsCollapsed = stateNodeWrapper.isCollapsed;
                currentGraph.SetNode(stateNode);

                for (int t = stateNodeWrapper.savedCondition.Count - 1; t >= 0; t--)
                {
                    TransitionNode transitionNode = AddTransitionNode(
                        stateNodeWrapper.savedCondition[t].position,
                        stateNodeWrapper.savedCondition[t].transition, stateNode);

                    transitionNode.targetCondition = 
                        stateNodeWrapper.savedCondition[t].condition;
                }
            }
        }

        private static void LoadTransitions()
        {
            // Load Transitions
        }

        #endregion
    }
}
