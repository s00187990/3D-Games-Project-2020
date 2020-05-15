using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLook : MonoBehaviour
{

    public float horizontalSpeed;
    public float verticalSpeed;
    public Camera lookCamera;
    public GameObject hand;
    private float upRotation;
    public float maxLookDownAngle = 40;
    public float maxLookUpAngle = 25;
    public Crosshair Crosshair;
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
  

    private void Update()
    {
        if (Input.GetKey(KeyCode.DownArrow))
        {
            Cursor.visible = !Cursor.visible;
            Cursor.lockState = Cursor.visible ? CursorLockMode.None : CursorLockMode.Locked;
        }

        float mouseX = Input.GetAxis("Mouse X") * horizontalSpeed * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * verticalSpeed * Time.deltaTime;

        upRotation -= mouseY;
        float upRotationHand = upRotation;
        upRotation = Mathf.Clamp(upRotation, -maxLookDownAngle, maxLookUpAngle);

        lookCamera.transform.localRotation = Quaternion.Euler(upRotation, 0, 0);
        Quaternion newHandRotation;
        newHandRotation = Quaternion.Euler(upRotationHand + 90,
            (Crosshair.raycastHit.point.y - hand.transform.localRotation.y)
            , Crosshair.raycastHit.point.normalized.z - hand.transform.localRotation.z);
        hand.transform.localRotation = newHandRotation;
        transform.Rotate(Vector3.up * mouseX);

    }
}

