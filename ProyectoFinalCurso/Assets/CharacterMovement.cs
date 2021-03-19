using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform box;

    public float speed = 6f;
    public float turnSpeed = 1000f;

    private bool lookingRight = true;
    float yRotation = 90;


    private 

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, vertical, 0f).normalized;

        if(direction.magnitude >= 0.1f)
        {

            if(horizontal < 0f)
            {
                lookingRight = true;
            }
            else if(horizontal > 0f)
            {
                lookingRight = false;
            }

            controller.Move(direction * speed * Time.deltaTime);
        }

        if(lookingRight && yRotation < 90)
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
