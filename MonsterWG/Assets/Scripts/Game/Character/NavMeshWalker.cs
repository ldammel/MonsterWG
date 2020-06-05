using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshWalker : MonoBehaviour
{
    private NavMeshAgent _agent;
    [SerializeField] private Transform[] waypoints;

    private int _nextPoint;

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _nextPoint = 0;
    }

    void Update()
    {
        if (waypoints == null) return;

        if (Vector3.Distance(waypoints[_nextPoint].position, transform.position) > 1)
        {
            _agent.SetDestination(waypoints[_nextPoint].position);
        }
        else
        {
            if (_nextPoint < waypoints.Length-1) _nextPoint++;
            else _nextPoint = 0;
        }
    }
}
