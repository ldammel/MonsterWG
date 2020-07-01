using UnityEngine;
using UnityEngine.AI;

namespace Game.Character
{
    public class NavMeshWalker : MonoBehaviour
    {
        public NavMeshAgent agent;
        public bool canContinue = true;
        [SerializeField] private Transform[] waypoints;
        private int _nextPoint;

        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            agent.isStopped = false;
            canContinue = true;
        }

        private void Update()
        {
            if (waypoints == null) return;
            if (agent.isStopped) return;

            if (Vector3.Distance(waypoints[_nextPoint].position, transform.position) > 1)
            {
                agent.SetDestination(waypoints[_nextPoint].position);
            }
            else
            {
                if (_nextPoint < waypoints.Length-1) _nextPoint++;
                else _nextPoint = 0;
                agent.isStopped = true;
            }
        }

        public void StopWalker()
        {
            agent.isStopped = true;
            canContinue = false;
        }
    }
}
