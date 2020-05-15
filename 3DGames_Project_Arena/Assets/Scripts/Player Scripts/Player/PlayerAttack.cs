using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public delegate void PlayerFired(int hitCounter);
public delegate void PlayerDropedWeapon();
public class PlayerAttack : MonoBehaviour
{

    public PickupState PickupState;
    public Weapon currentWeapon;
    public string FireButton = "Fire1";
    public string DropWeaponButton = "Fire2";
    public Transform FirePoint;
    private int hitCounter = 0;
    public Crosshair Crosshair;
    private float timer;
    public static event PlayerFired OnPlayerFired;
    public static event PlayerDropedWeapon OnPlayerWeaponDroped;

    private Vector3 hitPos { get { return Crosshair.raycastHit.point == Vector3.zero ? FirePoint.up : (Crosshair.raycastHit.point - FirePoint.position).normalized; } }
    void Update()
    {

        PickupState = GetComponent<Pickup>().pickupState;
        // fire
        if (Input.GetButtonUp(FireButton))
        {
            if (PickupState == PickupState.HasWeapon)
            {
                currentWeapon = GetComponent<Pickup>().currentWeapon.GetComponent<Weapon>();
                hitCounter++;
                //print($"PlayerAttack: hit pos: {hitpos}");
                currentWeapon.Fire(hitPos, FirePoint.rotation);

                if (OnPlayerFired!=null)  OnPlayerFired(hitCounter);
                //currentWeapon.Fire(CrosshairLandMark);
                if (hitCounter >= currentWeapon.Durability)
                {
                    DropWeapon(currentWeapon.destroyAfter);
                }
            }
        }
        // drop
        if (Input.GetButtonUp(DropWeaponButton))
        {
            if (PickupState == PickupState.HasWeapon)
            {
                DropWeapon();
            }
        }
    }

    private void DropWeapon()
    {
        DropWeapon(0);
    }
    private void DropWeapon(float destroyAfter)
    {
        if (OnPlayerWeaponDroped != null) OnPlayerWeaponDroped();
        currentWeapon = GetComponent<Pickup>().currentWeapon.GetComponent<Weapon>();
        hitCounter = 0;
        Destroy(currentWeapon.gameObject, destroyAfter);
        GetComponent<Pickup>().pickupState = PickupState.Empty;
    }
}
