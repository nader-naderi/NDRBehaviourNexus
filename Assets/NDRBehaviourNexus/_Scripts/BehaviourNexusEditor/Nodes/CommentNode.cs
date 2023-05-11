using UnityEngine;

namespace NDRBehaviourNexus
{
    public class CommentNode : BaseNode
    {
        string comment = "I'm a new comment.";
        public override void DrawCurve()
        {

        }

        public override void DrawWindow()
        {
            comment = GUILayout.TextArea(comment, 200);
        }
    }
}
