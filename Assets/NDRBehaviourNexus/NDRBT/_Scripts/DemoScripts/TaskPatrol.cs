using UnityEngine;

namespace NDRBT
{
    public class TaskPatrol : Node
    {
        private Transform transform;
        private Transform[] _waypoints;
        private int _currentWaypointIndex = 0;

        private float _waitTime = 1f;
        private float _waitCounter = 0;
        private bool _waiting = false;


        public TaskPatrol(Transform transform, Transform[] waypoints)
        {
            this.transform = transform;
            _waypoints = waypoints;
        }

        public override ENodeState Evaluate()
        {
            if (_waiting)
            {
                _waitCounter += Time.deltaTime;
                if (_waitCounter >= _waitTime)
                    _waiting = false;
            }
            else
            {
                Transform wp = _waypoints[_currentWaypointIndex];
                if (Vector3.Distance(transform.position, wp.position) < 0.01f)
                {
                    transform.position = wp.position;
                    _waitCounter = 0f;
                    _waiting = true;

                    _currentWaypointIndex = (_currentWaypointIndex + 1) % _waypoints.Length;
                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, wp.position, 2f * Time.deltaTime);
                    transform.LookAt(wp.position);
                }
            }

            state = ENodeState.RUNNING;
            return state;
        }
    }
}
