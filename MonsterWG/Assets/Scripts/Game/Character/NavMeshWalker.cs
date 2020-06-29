using UnityEngine;
using UnityEngine.AI;

namespace Game.Character
{
    public class NavMeshWalker : MonoBehaviour
    {
        public NavMeshAgent _agent;
        [SerializeField] private Transform[] waypoints;
        private int _nextPoint;

        private void Start()
        {
            _agent = GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            if (waypoints == null) return;
            if (_agent.isStopped) return;

            if (Vector3.Distance(waypoints[_nextPoint].position, transform.position) > 1)
            {
                _agent.SetDestination(waypoints[_nextPoint].position);
            }
            else
            {
                if (_nextPoint < waypoints.Length-1) _nextPoint++;
                else _nextPoint = 0;
                _agent.isStopped = true;
            }
        }
    }
}
