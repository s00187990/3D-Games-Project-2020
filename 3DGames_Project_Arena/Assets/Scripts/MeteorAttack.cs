using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MeteorAttack : MonoBehaviour
{
    public GameObject MeteorPrefab;
    public Transform[] Stars;
    public Spin Spin;

    public float ScaleingSpeed = 0.001f;
    Vector3 ScaleVectorSpeed { get { return new Vector3(ScaleingSpeed, ScaleingSpeed, ScaleingSpeed); } }

    public float Damage;
    public float MeteorSpeed;
    public float CoolDown;
    public float numberOfAttacks;
    float timeToSpawn;
    float counter;
    
    public enum state { Spinning, Attacking, CoolDown }
    GameObject Player;
    public state current;


    private void Start()
    {
        Player = GameObject.Find("Player");
        timeToSpawn = Time.time + CoolDown;
    }

    private void Update()
    {
        switch (current)
        {
            case state.Spinning:
                // just waiting 
                if (!Spin.spin)
                {
                    Spin.spin = true;
                    Spin.speed = 0.2f;
                }
                break;
            case state.Attacking:
                if (Spin.speed > 0)
                {
                    Spin.spin = true;
                    Spin.speed = -0.2f;
                }
                foreach (var star in Stars)
                {
                    star.transform.localScale -= (ScaleVectorSpeed*2) * Time.deltaTime;
                    if(star.transform.localScale.x <= Vector3.zero.x)
                    {
                        star.transform.localScale = Vector3.zero;
                    }
                }
                if (timeToSpawn <= Time.time)
                {
                    // Debug.Log("Boom!!" + counter);
                    ShootOne();
                    if (counter >= numberOfAttacks)
                    {
                        counter = 0;
                        current = state.CoolDown;
                        Spin.spin = false;
                    }
                }
                break;
            case state.CoolDown:
                foreach (var star in Stars)
                {
                    star.transform.localScale += ScaleVectorSpeed/2 * Time.deltaTime;
                    if (star.transform.localScale.x >= 2)
                    {
                        star.transform.localScale = Vector3.one *2;
                    }
                }
                if (Stars[Stars.Length - 1].transform.localScale == Vector3.one * 2)
                    current = state.Spinning;
                break;
        }
    }

    private void ShootOne()
    {
        GameObject projectile = Instantiate(MeteorPrefab, transform.position, transform.rotation);
        projectile.GetComponent<Projectile>().Direction =  Player.transform.position - transform.position ;
        projectile.GetComponent<Rigidbody>().useGravity = false;
        projectile.GetComponent<Projectile>().Damage = Damage;
        projectile.GetComponent<Projectile>().Speed = MeteorSpeed;
        projectile.SetActive(true);
        counter++;
        timeToSpawn = Time.time + CoolDown;
    }

    public void RainHell(int howManyTimes)
    {
        current = state.Attacking;
        numberOfAttacks = howManyTimes;
    }

}
