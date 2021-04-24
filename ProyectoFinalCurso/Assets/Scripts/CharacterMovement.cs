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
    public float jumpHeight = 3f;
        //Private
    Vector3 velocity; //vertical downwards velocity
    bool isGrounded;

    //World Rotation
    bool isASide = true;
    bool canMove = true;


    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(groundCheck.position, new Vector3(0.65f, groundDistance, 0.1f));
    }

    private void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        Vector3 direction = new Vector3(horizontal, 0f, 0f).normalized;
        if(!isASide){
            direction *= -1;
        }
        if(direction.magnitude >= 0.1f && canMove)
        {

            if(horizontal < 0f)//L
            {
                lookingRight = false;
            }
            else if(horizontal > 0f)//R
            {
                lookingRight = true;
            }

            controller.Move(direction * speed * Time.deltaTime);
        }
        
        //Jump
        {
            if (Input.GetButtonDown("Jump") && isGrounded && canMove)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
            }
        }

        //gravity
        {
            isGrounded = Physics.CheckBox(groundCheck.position, new Vector3(0.65f, groundDistance, 0.1f),  new Quaternion(0,0,0,0), groundMask);

            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -0f;
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

    //checks if character is on A side or B side on the map
    public void IsASide()
    {
        isASide = !isASide;

       /// Debug.Log("girado" + isASide);

        if(isASide)
        {
            Debug.Log("A");
            //transform.position = new Vector3(transform.position.x, transform.position.y, -0.35f);
            controller.Move(new Vector3(0, 0, transform.position.z - 1.1f));
        }
        else if(!isASide)
        {
            Debug.Log("B");
            //transform.position = new Vector3(transform.position.x, transform.position.y, 0.45f);
            controller.Move(new Vector3(0, 0, transform.position.z + 1.1f));
        }
    }

    public void CanPlayerMove(bool _canMove)
    {
        canMove = _canMove;
    }
}
