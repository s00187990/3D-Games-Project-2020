using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]

public class AINAVMovement : EnemyBase
{
    public bool PlayeMovingSoundEffect { get; set; } = false;
    public NavMeshAgent Agent;
    public bool PlayerChangedPos { get { return Agent.destination != PlayerPosition; } }
    public Vector3 PlayerPosition { 
        get 
        {
            if (Target == null ) return transform.position;
            Vector3 playerPosition = Target.transform.position;
            return playerPosition; 
        }
    }
    public override void Start()
    {
        base.Start();
        Agent = GetComponent<NavMeshAgent>();
    }

    public void Move(Vector3 _destination)
    {
        Agent.isStopped = false;
        Agent.SetDestination(_destination);
        _EnemyCurrentState = EnemyState.Moving;
        PlayeMovingSoundEffect = true;
    }

    public void StopMoving(EnemyState _enemState)
    {
        Agent.isStopped = true;
        _EnemyCurrentState = _enemState;
    }

    public void ResumeMoving()
    {
        Agent.isStopped = false;
        _EnemyCurrentState = EnemyState.Moving;
    }
    
    
}
