using System;
using Game.Quests;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace Game.Character
{
    public class NavMeshWalker : MonoBehaviour
    {
        public static NavMeshWalker instance;

        private void Awake()
        {
            if (instance != null)
            {
                Debug.LogError("More than one instance of NavMeshWalker!");
                Application.Quit();
            }

            instance = this;
        }

        public NavMeshAgent agent;
        public bool canContinue = true;
        [SerializeField] private Transform[] waypoints;
        [SerializeField] private GameObject scoreScreen2;
        private int _nextPoint;
        public bool FinalWayPoint { get; set; }
        public bool MiniQuest;
        public UnityEvent miniQuestEvent;
        

        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            agent.isStopped = false;
            canContinue = true;
        }

        private void Update()
        {
            if (waypoints == null) return;
            if (FinalWayPoint)
            {
                scoreScreen2.SetActive(true);
                StopWalker();
            }
            if (agent.isStopped) return;

            if (Vector3.Distance(waypoints[_nextPoint].position, transform.position) > 1)
            {
                agent.SetDestination(waypoints[_nextPoint].position);
            }
            else
            {
                if (MiniQuest)
                {
                    agent.isStopped = true;
                    FinalScoring.instance.StartMiniQuest();
                    return;
                }
                if (_nextPoint < waypoints.Length-1) _nextPoint++;
                else
                {
                    _nextPoint = 0;
                    FinalWayPoint = true;
                }
                agent.isStopped = true;
            }
        }
        
        public void StartMiniQuest() 
        {
            MiniQuest = true;
            miniQuestEvent.Invoke();
        }

        public void StopWalker()
        {
            agent.isStopped = true;
            canContinue = false;
        }
    }
}
