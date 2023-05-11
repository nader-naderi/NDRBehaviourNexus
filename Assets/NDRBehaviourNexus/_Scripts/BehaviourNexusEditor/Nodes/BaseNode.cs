using UnityEngine;

namespace NDRBehaviourNexus
{
    public abstract class BaseNode : ScriptableObject
    {
        public Rect windowRect;
        public string windowTitle;

        public abstract void DrawWindow();
        public abstract void DrawCurve();

    }
}