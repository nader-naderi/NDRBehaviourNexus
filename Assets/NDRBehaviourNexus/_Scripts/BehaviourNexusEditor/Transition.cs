namespace NDRBehaviourNexus
{
    [System.Serializable]
    public class Transition
    {
        public Condition Condition;
        public State TargetState;
        public bool IsDisbale;
    }
}
