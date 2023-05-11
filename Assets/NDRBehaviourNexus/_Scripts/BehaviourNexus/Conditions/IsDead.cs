using NDRBehaviourNexus.TestChamber;

using UnityEngine;

namespace NDRBehaviourNexus
{
    [CreateAssetMenu(fileName = "IsDead", menuName = "Conditions/IsDead")]
    public class IsDead : Condition
    {
        public override bool CheckCondition(Pawn pawn)
        {
            return pawn.Health <= 0;
        }
    }
}