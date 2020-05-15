using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : Weapon
{
    public float Range;
    public LayerMask EnemyLayerMask;
    public Animator Animator;
    public int Hits = 1;


    protected override void Start()
    {
        base.Start();
        WeaponType = WeaponType.Melee;
    }

    public override void Fire(Vector3 direction)
    {
        
        Animator.SetTrigger("Attack");
        Collider[] colliders = new Collider[5];
        colliders = Physics.OverlapSphere(transform.position, Range, EnemyLayerMask);
        int hits = 0;
        foreach (var item in colliders)
        {
            Health targetHealth = item.transform.GetComponent<Health>();
            if (targetHealth != null)
            {
                targetHealth.TakeDamage(Damage);
                //print("Hit -" + item.transform.name);
                hits++;
                if (hits >=Hits)
                    break;
            }
            continue;
        }

    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, Range);
    }
}
