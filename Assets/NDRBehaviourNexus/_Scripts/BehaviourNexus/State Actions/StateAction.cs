using NDRBehaviourNexus.TestChamber;

using UnityEngine;

namespace NDRBehaviourNexus
{
    public abstract class StateAction : ScriptableObject
    {
        public abstract void Execute(Pawn pawn);
    }
}