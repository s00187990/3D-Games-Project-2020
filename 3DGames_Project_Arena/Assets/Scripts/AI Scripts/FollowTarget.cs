using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : AINAVMovement
{
    
    public override void Update()
    {
        base.Update();

        switch (EnemyCurrentState)
        {
            case EnemyState.Moving:
                if(Agent.destination != Target.transform.position)
                {
                    Move(Target.transform.position);

                    float Distance = Vector3.Distance(transform.position, Target.transform.position);
                    if (Distance <= AttackDistance)
                    {
                        StopMoving(EnemyState.Attacking);
                    }
                }
                break;
            case EnemyState.Attacking:
                float CurrentDistance = Vector3.Distance(transform.position, Target.transform.position);
                if (CurrentDistance > AttackDistance) //(CurrentDistance < VisionDistance )
                {
                    Agent.isStopped = false;
                    Move(Target.transform.position);
                }
                break;
            case EnemyState.Dead:
                break;
        }
    }
}
