using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PickupType
{
    Collectable,
    Consumable
}
public class Collectable : MonoBehaviour
{
    public PickupType PickupType {  get; protected set; }
    public string Name = "nothing";
}
