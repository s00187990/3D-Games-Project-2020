using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Movement : MonoBehaviour
{
    public float horizontalSpeed;
    public float verticalSpeed;
    public float gravitySpeed = 10f;
    public float jumpHeight = 8f;
    public float groundCheckDis;
    public LayerMask groundCheckMask;
    public LayerMask groundDamageMask;
    public float GroundDamage;
    public Transform groundCheckTransform;

    bool isGrounded;
    private CharacterController controller;
    private Vector3 velocity = Vector3.zero;


    private void Start()
    {
        controller = GetComponent<CharacterController>();
        //GetComponent<Health>().OnDeath += Movement_OnDeath;

    }

    //private void Movement_OnDeath()
    //{
    //    this.gameObject.SetActive(false);
    //}

    private void Update()
    {

        Vector3 moveDirection;
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        moveDirection = transform.right * horizontal;
        moveDirection.Normalize();
        controller.Move(moveDirection * Time.deltaTime * horizontalSpeed);

        moveDirection = transform.forward * vertical;
        moveDirection.Normalize();
        controller.Move(moveDirection * Time.deltaTime * verticalSpeed);


        // checking for ground 
        isGrounded = Physics.CheckSphere(groundCheckTransform.position, groundCheckDis, groundCheckMask);
        // checking for ground damage
        if (Physics.CheckSphere(groundCheckTransform.position, groundCheckDis, groundDamageMask))
        {
            //print("Player Is on ground damage");
            GetComponent<Health>().TakeDamage(GroundDamage);
        }

        if (isGrounded && velocity.y < 0)
            velocity.y = -jumpHeight;

        velocity.y += gravitySpeed * Time.deltaTime;

        if (gravitySpeed > 0)
            gravitySpeed *= -1;

        // jump controll 
        if (isGrounded && Input.GetButton("Jump"))
        {
            //Debug.Log("Jump");
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravitySpeed);
        }

        controller.Move(velocity * Time.deltaTime);

    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(groundCheckTransform.position, groundCheckDis);

    }




}
