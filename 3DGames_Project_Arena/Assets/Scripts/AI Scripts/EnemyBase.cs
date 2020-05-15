using UnityEngine;

public enum EnemyState
{
    Moving,
    Attacking,
    Dead
}

public class EnemyBase : MonoBehaviour
{
    public float FieldOfView = 45;
    public float VisionDistance = 50;
    public float AttackDistance;
    public string TargetTag = "Player";
    public LayerMask TargetMask;
    public Transform CenterPoint;
    public EnemyState EnemyCurrentState
    {
        get { return _EnemyCurrentState; }
        
    }
    protected GameObject Target;
    protected EnemyState _EnemyCurrentState;
    protected bool TargetInFOV = false;
    public virtual void Start()
    {
        Target = GameObject.FindGameObjectWithTag(TargetTag);

    }

    public virtual void Update()
    {
        TrackTarget();
    }

    public void TrackTarget()
    {
        //Collider[] TargetHits;
        //TargetHits = Physics.OverlapSphere(CenterPoint.position, VisionDistance, TargetMask);
        if (Physics.Raycast(CenterPoint.position, // origin 
            (Target.transform.position - CenterPoint.position).normalized, // direction
            out RaycastHit TargetHit, VisionDistance, TargetMask))
        {
            // Find direstion of target
            Vector3 PlayerDirection = TargetHit.point - CenterPoint.position;
            // Calculates angle from z-axis to target position
            float Angle = Vector3.Angle(CenterPoint.forward, PlayerDirection);

            // Check if target is in FOV
            TargetInFOV = Angle <= FieldOfView;
        }
    }

    private void OnDrawGizmos()
    {

        // Sphere Vision
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(CenterPoint.position, VisionDistance);

        //Attack Distance 
        Gizmos.color = Color.red;
        Gizmos.DrawRay(CenterPoint.position, CenterPoint.forward.normalized * AttackDistance);

        // FOV Lines
        Vector3 Line1;
        Vector3 Line2;
        Line1 = Quaternion.AngleAxis(FieldOfView, CenterPoint.up) * CenterPoint.forward * VisionDistance;
        Line2 = Quaternion.AngleAxis(-FieldOfView, CenterPoint.up) * CenterPoint.forward * VisionDistance;

        // Draw FOV
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(CenterPoint.position, Line1);
        Gizmos.DrawRay(CenterPoint.position, Line2);

        // Draw Line to target
        if (TargetInFOV)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(CenterPoint.position, ((Target.transform.position + Vector3.up) - CenterPoint.position).normalized * VisionDistance);
        }


    }

    private void OnDestroy()
    {
        LevelManager.IDied(gameObject);
    }
}
