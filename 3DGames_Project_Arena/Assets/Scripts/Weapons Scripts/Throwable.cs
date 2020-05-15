using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : Weapon
{

    public float ThrowPower;
    public GameObject PrjectailPrefab;

    public override void Fire(Vector3 direction,Quaternion rotation)
    {
        //Quaternion rotation = Quaternion.Euler(direction);
        //rotation.x -= 90;
        GameObject Prjectail = Instantiate(PrjectailPrefab,transform.position, rotation);
        //Prjectail.GetComponent<Rigidbody>().AddForce((ThrowPower/3) * Vector3.up, ForceMode.Impulse);
        Prjectail.GetComponent<Projectile>().Direction =  direction;
        Prjectail.GetComponent<Projectile>().Speed = ThrowPower/2;
        Prjectail.GetComponent<Projectile>().Damage = Damage;
    }


}
