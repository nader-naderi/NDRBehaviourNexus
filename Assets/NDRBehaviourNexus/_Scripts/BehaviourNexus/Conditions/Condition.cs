using NDRBehaviourNexus.TestChamber;

using UnityEngine;

namespace NDRBehaviourNexus
{
    public abstract class Condition : ScriptableObject
    {
        public abstract bool CheckCondition(Pawn pawn);
    }
}