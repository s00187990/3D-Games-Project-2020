using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionDamage : MonoBehaviour
{
    public LayerMask EnemiesLayer;
    public float Damage = 5f;
    public float DestroyAfter = 3f;
    public float DamageRadius = 5f;
    // Start is called before the first frame update
    void Start()
    {
        Collider[] hits = new Collider[20];
        hits = Physics.OverlapSphere(this.transform.position, DamageRadius, EnemiesLayer);
        foreach (var item in hits)
        {
            Health hitHealth = item.gameObject.GetComponent<Health>();
            if (hitHealth == null) continue;
            //print(item.name);
            hitHealth.TakeDamage(Damage);
        }
        Destroy(this.gameObject, DestroyAfter);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, DamageRadius);
    }
}
