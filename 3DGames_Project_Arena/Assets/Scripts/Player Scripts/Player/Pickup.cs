using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PickupState
{
    Empty,
    HasWeapon
}

public delegate void PickedUp(GameObject pickedfUpWeapon);
public class Pickup : MonoBehaviour
{

    public Transform castStartPosition;
    public GameObject hand;
    public GameObject currentWeapon = null;
    public float castDistance = 50f;
    public PickupState pickupState = PickupState.Empty;
    public static event PickedUp OnPickup ;
    //public float xOffSet;

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.E))
        {
            if (pickupState == PickupState.HasWeapon) return;
            PickUp();
        }

    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(castStartPosition.position, (this.transform.forward /*- new Vector3(xOffSet, 0, 0)*/) * castDistance);
        Gizmos.DrawWireSphere(castStartPosition.position, castDistance);

    }

    private void PickUp()
    {
        Collider[] colliders;
        colliders = Physics.OverlapSphere(castStartPosition.position, castDistance);

        foreach (var item in colliders)
        {
            Collectable collectable = item.transform.gameObject.GetComponent<Collectable>();
            if (collectable == null) continue;
            if (collectable.PickupType == PickupType.Collectable)
            {
                //if (pickupState == PickupState.HasWeapon) return;
                GameObject pickedUp = Instantiate(item.transform.gameObject, hand.transform.position, hand.transform.rotation, hand.transform);
                pickedUp.GetComponent<Collider>().enabled = false;
                if (currentWeapon != null)
                    Destroy(currentWeapon.gameObject);
                currentWeapon = pickedUp;
                if (OnPickup != null)
                    OnPickup(pickedUp);
                pickupState = PickupState.HasWeapon;
                Destroy(item.transform.gameObject);
            }
        }
    }
}
