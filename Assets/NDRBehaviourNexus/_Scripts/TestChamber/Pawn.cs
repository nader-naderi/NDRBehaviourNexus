using UnityEngine;

namespace NDRBehaviourNexus.TestChamber
{
    public class Pawn : MonoBehaviour
    {
        [SerializeField] private float health = 100;
        [SerializeField] private State currentState;

        public float Health { get => health; }
        public State CurrentState { get => currentState; }

        public Transform Transform { get; private set; }
        public float Delta { get; private set; }

        private void Start()
        {
            Transform = transform;
        }

        private void Update()
        {
            UpdateStates();
        }

        private void UpdateStates()
        {
            if (!currentState)
                return;

            currentState.OnUpdate(this);
        }

        public void SetState(State state) => currentState = state;

        public void TakeDamage(float damage)
        {
            health -= damage;
        }

        public void AddDamage(float damage)
        {
            health += damage;
        }
    }
}
