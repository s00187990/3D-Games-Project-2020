using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum WeaponType
{
    Melee,
    Throwable
}

public class Weapon : Collectable
{
    public Sprite Image;
    public float Damage;
    public float destroyAfter;
    protected Rigidbody body { get { return GetComponent<Rigidbody>(); } }

    public int Durability = 1;

    public WeaponType WeaponType;
    protected virtual void Start()
    {
        this.PickupType = PickupType.Collectable;
    }

    public virtual void Fire(Vector3 direction)
    {

    }
    public virtual void Fire(Vector3 direction ,Quaternion rotation)
    {
        Fire(direction);
    }

}
