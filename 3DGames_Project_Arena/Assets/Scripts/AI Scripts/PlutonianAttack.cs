using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlutonianAttack : MonoBehaviour
{

    AINAVMovement MovementRef { get { return GetComponent<AINAVMovement>(); } }

    

    public AudioClip[] audioClipsMoving;
    public AudioClip[] audioClipsDeath;
    public Transform CenterPoint;
    public AudioSource AudioSource;
    public GameObject PrjectailPrefab;
    public Animator Animator;
    public float fireRate;
    


    float timeTofire;
    bool destroy = false;
    public GameObject Explosion;
    public float Damage;

     
    private void Start()
    {
        GetComponent<Health>().OnDeath += PlutonianAttack_OnDeath;
    }

    private void PlutonianAttack_OnDeath()
    {
        if (MovementRef.EnemyCurrentState == EnemyState.Dead) return;
        Animator.SetTrigger("Death");
        MovementRef.StopMoving(EnemyState.Dead);
        destroy = true;
    }

    private void Update()
    {

        EnemyState currentState = MovementRef.EnemyCurrentState;
        int i;
        switch (currentState)
        {
            case EnemyState.Moving:
               // print("I will get you");
                if (MovementRef.PlayeMovingSoundEffect && !AudioSource.isPlaying)
                {
                    i = Random.Range(0, audioClipsMoving.Length);
                    AudioSource.clip = audioClipsMoving[i];
                    AudioSource.Play();
                    MovementRef.PlayeMovingSoundEffect = false;
                }
                break;

            case EnemyState.Attacking:
                //print("Bam!! Bam!!");
                Shoot();
                Quaternion lookRotation = Quaternion.LookRotation(MovementRef.PlayerPosition - transform.position);
                lookRotation.x = transform.rotation.x;
                lookRotation.z = transform.rotation.z;
                transform.rotation = lookRotation;
                break;
            case EnemyState.Dead:
                if (destroy)
                {
                    print("this is not the End @£$% ");
                    // playe sound effect
                    i = Random.Range(0, audioClipsDeath.Length);
                    AudioSource.clip = audioClipsDeath[i];
                    AudioSource.Play();
                    Invoke("Explode", 0.2f);
                    Destroy(this.gameObject, AudioSource.clip.length);
                    destroy = false;
                }
                break;
        }
    }
    private void Explode()
    {
        Instantiate(Explosion, transform.position + (Vector3.up * (transform.localScale.y / 2)), Quaternion.identity);
    }

    private void Shoot()
    {
        if (timeTofire >= Time.time) return;
        Vector3 direction = MovementRef.PlayerPosition - CenterPoint.position;
        GameObject Prjectail = Instantiate(PrjectailPrefab, CenterPoint.position, CenterPoint.rotation);
        Prjectail.GetComponent<Projectile>().Direction = direction;
        Prjectail.GetComponent<Projectile>().Damage = Damage;
        timeTofire = Time.time + fireRate;
    }

    private void OnDrawGizmos()
    {
        if (CenterPoint == null ) return;
        Gizmos.color = Color.green;
        Gizmos.DrawLine(CenterPoint.position, (MovementRef.PlayerPosition));
    }

    private void OnDisable()
    {
        GetComponent<Health>().OnDeath -= PlutonianAttack_OnDeath;
    }


}
