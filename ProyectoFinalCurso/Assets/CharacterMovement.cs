using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public CharacterController controller;

    //character public atributes
    public Transform box;
    public float speed = 6f;
    public float turnSpeed = 1000f;
    public float gravity = -9.81f;

    //character private atributes
    private bool lookingRight = true;
    private float yRotation = 90;

    //Gravity - GroundCheck
    //Public
    public Transform groundCheck;
    public float groundDistance = 0.1f;
    public LayerMask groundMask;

    //
    public float jumpHeight = 3f;


    //Private
    Vector3 velocity; //vertical downwards velocity
    bool isGrounded;

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(groundCheck.position, new Vector3(0.95f, groundDistance, 0.1f));
    }

    private void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, 0f).normalized;

        

        if(direction.magnitude >= 0.1f)
        {

            if(horizontal < 0f)//I
            {
                
                lookingRight = false;
            }
            else if(horizontal > 0f)//D
            {
                lookingRight = true;
                
            }

            controller.Move(direction * speed * Time.deltaTime);
        }
        //Jump
        {
            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
            }
        }

        //gravity
        {
            isGrounded = Physics.CheckBox(groundCheck.position, new Vector3(0.95f, groundDistance, 0.1f),  new Quaternion(0,0,0,0), groundMask);

            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }

            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }

        //change sight direction (rotation)
        {
            if (lookingRight && yRotation < 90)
            {
                yRotation += turnSpeed * Time.deltaTime;

                if (yRotation > 90)
                    yRotation = 90;
            }
            else if (!lookingRight && yRotation > -90f)
            {
                yRotation -= turnSpeed * Time.deltaTime;

                if (yRotation < -90f)
                    yRotation = -90f;
            }

            box.rotation = Quaternion.Euler(new Vector3(0f, yRotation, 0f));
        }
    }
}
