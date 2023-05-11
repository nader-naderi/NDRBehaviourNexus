using NDRBehaviourNexus.TestChamber;

using UnityEngine;

namespace NDRBehaviourNexus
{
    [CreateAssetMenu(fileName = "ChangeHealth", menuName = "Actions/ChangeHealth")]
    public class ChangeHealth : StateAction
    {
        public override void Execute(Pawn pawn)
        {
            pawn.AddDamage(10);
        }
    }
}