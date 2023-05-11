using NDRBehaviourNexus.TestChamber;
using System.Collections.Generic;
using UnityEngine;

namespace NDRBehaviourNexus
{
    [CreateAssetMenu]
    public class State : ScriptableObject
    {
        public StateAction[] onState;
        public StateAction[] onEnter;
        public StateAction[] onExit;


        public List<Transition> transitions = new List<Transition>();

        public void OnEnter(Pawn pawn)
        {
            ExecuteActions(pawn, onEnter);
        }

        public void OnUpdate(Pawn pawn)
        {
            ExecuteActions(pawn, onState);
            CheckTransitions(pawn);
        }

        public void OnExit(Pawn pawn)
        {
            ExecuteActions(pawn, onExit);
        }

        public void ExecuteActions(Pawn pawn, StateAction[] actions)
        {
            for (int i = 0; i < actions.Length; i++)
            {
                if (actions[i])
                    actions[i].Execute(pawn);
            }
        }

        public void CheckTransitions(Pawn pawn)
        {
            for (int i = 0; i < transitions.Count; i++)
            {
                if (transitions[i].IsDisbale)
                    continue;

                if (transitions[i].Condition.CheckCondition(pawn))
                {
                    if (transitions[i].TargetState != null)
                    {
                        pawn.SetState(transitions[i].TargetState);
                        OnExit(pawn);
                        pawn.CurrentState.OnEnter(pawn);
                    }

                    return;
                }
            }
        }

        public Transition AddTransition()
        {
            Transition newTransition = new Transition();
            transitions.Add(newTransition);
            return newTransition;
        }
    }
}