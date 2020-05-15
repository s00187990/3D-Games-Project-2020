using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(AudioSource))]
public class Projectile : MonoBehaviour
{


    public string EnemyTag;
    public float Damage;
    public float Speed;
    public string IgnoreTag1;
    public string IgnoreTag;
    public GameObject EffectImpact;
    public bool DestroyOnImpact = false;
    public float DestroyAfter = 1;

    Vector3 direction;
    Rigidbody body { get { return this.GetComponent<Rigidbody>(); } }
    AudioSource Source;
    bool move = true;


    private void Start()
    {
        Source = GetComponent<AudioSource>();
        if (Source.clip != null)
            Source.Play();
    }


    public Vector3 Direction
    {
        get
        {
            if (direction == Vector3.zero)
                return transform.forward;
            else return direction;
        }
        set
        {
            direction = value;
        }
    }

    private void Update()
    {
        if (move)
        {
            transform.up = direction;
            body.velocity = Direction * Speed;
        }
        //Debug.Log($"not moving :(");
    }


    private void OnTriggerEnter(Collider collision)
    {

        if (collision.gameObject.tag.Equals(IgnoreTag1) || 
            collision.gameObject.tag.Equals(IgnoreTag) ||
            collision.gameObject.tag.Equals("Projectile"))
        {
            //Debug.Log($"Projectile: We hit but ignored: {collision.transform.name} - On layer: {collision.gameObject.layer}");
            return;
        }
        if (move)
        {
            move = false;
            body.freezeRotation = true;
            body.constraints = RigidbodyConstraints.FreezeAll;
            body.velocity = Vector3.zero;
            body.useGravity = false;
            body.isKinematic = true;

            //transform.position += collision.transform.position;
            //Debug.Log($"Projectile- not ignored : i am: {transform.name} - hit layer: {LayerMask.LayerToName(collision.gameObject.layer)}");
            if (collision.gameObject.tag == EnemyTag)
            {
                collision.gameObject.GetComponent<Health>().TakeDamage(Damage);
                transform.parent = collision.transform;
            }
            else
            {
                if (EffectImpact != null) 
                    Instantiate(EffectImpact, transform.position, Quaternion.identity, transform.parent);
            }
            Destroy();
        }
    }


    private void Destroy()
    {
        if (DestroyOnImpact) Destroy(this.gameObject);
        else Destroy(this.gameObject, DestroyAfter);
    }

}
