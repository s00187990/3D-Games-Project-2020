
using UnityEngine;

public enum AttackingState
{
    Nothing,
    All,
    NormalAttack,
    Spawning,
    StarAttack
}
public class MainBoss : MonoBehaviour
{

    public float NormalDamage = 20;
    public float NormalFireRate = 1;
    public float MaxNormalAttackDuration = 10;
    public AudioClip[] HurtSoundEffects;
    public AudioClip SpawnSoundEffects;
    public AudioClip MeteorAttackSoundEffects;

    public MeteorAttack MeteorAttack;
    public Spawner EnemySpawner;
    public Transform CastPosition;
    public Animator Animator;
    public Transform[] Arms;
    public Transform[] NormalFirePoint;
    public MeshRenderer BodyRenderer;
    public GameObject NormalAttackPrefab;
    public AudioSource AudioSource;

    private float NormalAttackDuration ;
    private Color NormalColour;
    float NormalAttackTimer;
    float SwitchAttackTimer;
    float SpawningDurationTimer;
    AttackingState CurrentAttackingState = AttackingState.NormalAttack;
    EnemyState CurrentState;
    bool playAttackSoundEffect = true;

    AINAVMovement MovementRef { get { return GetComponent<AINAVMovement>(); } }

    private void Start()
    {
        GetComponent<Health>().OnTakingDamage += MainBoss_OnTakingDamage;
        GetComponent<Health>().OnDeath += MainBoss_OnDeath;
        NormalAttackDuration = MaxNormalAttackDuration;
        SwitchToNormalAttack();
    }


    private void MainBoss_OnDeath()
    {
        print("You are not worthy");
        GetComponent<Health>().OnTakingDamage -= MainBoss_OnTakingDamage;
        GetComponent<Health>().OnDeath -= MainBoss_OnDeath;
        Destroy(this.transform.parent.gameObject);
    }

    private void MainBoss_OnTakingDamage(float amount)
    {
        print("this hurts");
        int i;
        if(!AudioSource.isPlaying)
        {
            i = Random.Range(0, HurtSoundEffects.Length);
            AudioSource.clip = HurtSoundEffects[i];
            AudioSource.loop = false;
            AudioSource.Play();
        }
        NormalAttackDuration -= (amount/0.3f);
        NormalAttackDuration = Mathf.Clamp(NormalAttackDuration, 20, MaxNormalAttackDuration);
    }

    private void Update()
    {

        CurrentState = MovementRef.EnemyCurrentState;
        float Distance = Vector3.Distance(transform.position, MovementRef.PlayerPosition);

        switch (CurrentState)
        {
            case EnemyState.Moving:
                if (CurrentAttackingState == AttackingState.NormalAttack)
                {
                    // if in attacking distance and in normal attack stop and attack
                    if (Distance < MovementRef.AttackDistance)
                        Stop(EnemyState.Attacking);
                    else if (MovementRef.PlayerChangedPos)
                        Move(MovementRef.PlayerPosition);
                }
                if (IsAtDestination(MovementRef.Agent.destination))
                    Stop(EnemyState.Attacking);
                break;

            case EnemyState.Attacking:
                switch (CurrentAttackingState)
                {
                    case AttackingState.Nothing:
                        break;
                    case AttackingState.NormalAttack:
                        //if got close get back 
                        if (Distance < 6)
                        {
                            MovementRef.Agent.isStopped = false;
                            MovementRef.Agent.SetDestination(transform.position + ((transform.forward.normalized) * (MovementRef.AttackDistance - 5)) * -1);
                        }
                        Shoot();
                        //if got far get closer 
                        if (Distance > MovementRef.AttackDistance)
                            Move(MovementRef.PlayerPosition);

                        if (SwitchAttackTimer < Time.time)
                        {
                            ChangeToEventAttack();
                        }
                        break;
                    case AttackingState.Spawning:
                        // stand far and order the spawner to start spawning 
                        if (!IsAtDestination(CastPosition.position))
                            Move(CastPosition.position);
                        else
                        {
                            if (EnemySpawner.enabled)
                            {
                                if (SpawningDurationTimer < Time.time)
                                {
                                    EnemySpawner.enabled = false;
                                    SwitchToNormalAttack();
                                }
                            }
                            else
                            {
                                if (!AudioSource.isPlaying && playAttackSoundEffect)
                                {
                                    AudioSource.clip = SpawnSoundEffects;
                                    AudioSource.loop = false;
                                    AudioSource.Play();
                                    playAttackSoundEffect = false;
                                }
                                EnemySpawner.enabled = true;
                                SpawningDurationTimer = Time.time + 20;
                            }
                        }
                        break;

                    case AttackingState.StarAttack:
                        if (MeteorAttack.current == MeteorAttack.state.CoolDown)
                        {
                            Move(MovementRef.PlayerPosition);
                            SwitchToNormalAttack();
                            break;
                        }

                        // go to star and do the animation then start the Meteor Attack
                        if (!IsAtDestination(CastPosition.position))
                            Move(CastPosition.position);
                        else
                        {
                            print("Stars attack");
                            if (!AudioSource.isPlaying && playAttackSoundEffect)
                            {
                                AudioSource.clip = MeteorAttackSoundEffects;
                                AudioSource.loop = false;
                                AudioSource.Play();
                                playAttackSoundEffect = false;
                            }
                            if (MeteorAttack.current == MeteorAttack.state.Spinning)
                                MeteorAttack.current = MeteorAttack.state.Attacking;
                        }
                        break;
                }
                break;
            case EnemyState.Dead:
                break;
        }// end switch Moving state


    }

    private void ChangeToEventAttack()
    {
        int i = Random.Range(0, 100);
        CurrentAttackingState = i > 50f ? AttackingState.Spawning : AttackingState.StarAttack;
        NormalColour = BodyRenderer.material.GetColor("_EmissiveColor");
        BodyRenderer.material.SetColor("_EmissiveColor", new Color32(255, 0, 83, 255));

        playAttackSoundEffect = true;
    }

    private void SwitchToNormalAttack()
    {
        Move(MovementRef.PlayerPosition);
        CurrentAttackingState = AttackingState.NormalAttack;
        SwitchAttackTimer = Time.time + NormalAttackDuration;
        BodyRenderer.material.SetColor("_EmissiveColor", NormalColour);
    }

    bool IsAtDestination(Vector3 destination)
    {
        float Distance = Vector3.Distance(transform.position, destination);
        return Distance <= 5f;
    }

    private void LateUpdate()
    {
        if (CurrentState == EnemyState.Attacking)
        {
            Quaternion newRotation = Quaternion.LookRotation(MovementRef.PlayerPosition - transform.position);
            newRotation.x = Quaternion.identity.x;
            newRotation.z = 0;
            //newRotation.x = 0;
            transform.rotation = newRotation;
        }

        switch (CurrentAttackingState)
        {
            case AttackingState.NormalAttack:
                foreach (var arm in Arms)
                    arm.LookAt(MovementRef.PlayerPosition);
                break;
        }

    }

    private void Move(Vector3 destination)
    {
        Animator.SetBool("Moving", true);
        MovementRef.Move(destination);
    }


    private void Stop(EnemyState newState)
    {
        Animator.SetBool("Moving", false);
        MovementRef.StopMoving(newState);
    }


    private void Shoot()
    {
        if (NormalAttackTimer >= Time.time) return;

        foreach (var point in NormalFirePoint)
            ShootFrom(point);
        NormalAttackTimer = Time.time + NormalFireRate;
    }

    private void ShootFrom(Transform firePoint)
    {
        Vector3 direction = MovementRef.PlayerPosition - firePoint.position;
        GameObject Prjectail = Instantiate(NormalAttackPrefab, firePoint.position, firePoint.rotation);
        Prjectail.GetComponent<Projectile>().Direction = direction;
        Prjectail.GetComponent<Projectile>().Damage = NormalDamage;
    }
}
