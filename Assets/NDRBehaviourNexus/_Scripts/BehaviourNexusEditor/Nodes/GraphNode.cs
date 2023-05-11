using UnityEditor;

namespace NDRBehaviourNexus
{
    public class GraphNode  : BaseNode
    {
        BehaviourGraph previousGraph;

        public override void DrawCurve()
        {

        }

        public override void DrawWindow()
        {
            if (BehaviourEditor.currentGraph == null)
                EditorGUILayout.LabelField("Add Graph to modify: ");

            BehaviourEditor.currentGraph = (BehaviourGraph)EditorGUILayout.ObjectField(BehaviourEditor.currentGraph, typeof(BehaviourGraph), false);
       
            if (BehaviourEditor.currentGraph == null)
            {
                if (previousGraph != null)
                {
                    previousGraph = null;
                }

                EditorGUILayout.LabelField("No Graph Assigned");

                return;
            }

            if (previousGraph != BehaviourEditor.currentGraph)
            {
                previousGraph = BehaviourEditor.currentGraph;
                BehaviourEditor.LoadGraph();
            }

        }
    }
}