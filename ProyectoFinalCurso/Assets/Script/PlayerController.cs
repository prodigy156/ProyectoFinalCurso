using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float playerSpeed;
    public float rotationSpeed;
    public float jumpForce;
    private bool flip;
    public GameObject box;
    private Quaternion rotation;
    private float yRotation;
    private float inputValue;
    private int LastKey = 0; //0 = ninguno(inicio de juego), 1 = izquierda, 2 = derecha
    private bool canJump;
    private Rigidbody playerRb;

    void Start()
    {
        yRotation = 0;
        playerRb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        inputValue = Input.GetAxis("Horizontal");

        Vector3 move = new Vector3(0, 0, inputValue);

        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            canJump = false;
            playerRb.AddForce(0, jumpForce, 0, ForceMode.Impulse);

        }

        if (inputValue < 0 && !flip)
        {
            Debug.Log(inputValue);

            yRotation += rotationSpeed * Time.deltaTime;
            LastKey = 1;

            if (yRotation >= 180)
            {
                yRotation = 180;
                flip = true;
            }
        }
        else if (inputValue > 0 && flip)
        {
            yRotation -= rotationSpeed * Time.deltaTime;
            LastKey = 2;

            if (yRotation <= 0)
            {
                yRotation = 0;
                flip = false;
            }
        }
        else
        {
            if (LastKey == 1 && yRotation < 180)
            {
                yRotation += rotationSpeed * Time.deltaTime;
                if (yRotation >= 180)
                {
                    yRotation = 180;
                    flip = true;
                }
            }
            else if (LastKey == 2 && yRotation > 0)
            {
                yRotation -= rotationSpeed * Time.deltaTime;

                if (yRotation <= 0)
                {
                    yRotation = 0;
                    flip = false;
                }
            }
        }
        rotation = Quaternion.Euler(0, yRotation, 0);
        box.transform.rotation = rotation;
        this.transform.position += (move * Time.deltaTime * playerSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        canJump = true;
    }
    private void OnTriggerExit(Collider other)
    {
        canJump = false;
    }
}
