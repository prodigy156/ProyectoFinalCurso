using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float playerSpeed;
    private float _playerSpeed;
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

    public GameManager gameManager;



    float timer = 0;
    bool canMove;
    bool ASide;

    float z = 0.6f;

    void Start()
    {
        yRotation = 90;
        playerRb = GetComponent<Rigidbody>();
        _playerSpeed = playerSpeed;

        
    }

    void Update()
    {
        Vector3 move = Vector3.zero;
        timer += Time.deltaTime;
        if (canMove)
        {
            inputValue = Input.GetAxis("Horizontal");

            if (!gameManager.aSide)
            {
                inputValue *= -1;
            }
            move = new Vector3(inputValue, 0, 0);

            bool jump = Input.GetKey(KeyCode.Space);

            //transform.position += new Vector3(inputValue * playerSpeed * Time.deltaTime, 0, 0);

            if (Input.GetKeyDown(KeyCode.Space) && canJump)
            {
                playerRb.AddForce(0, jumpForce, 0, ForceMode.Impulse);
            }

            if (canJump == false)
            {
                playerSpeed = _playerSpeed / 2;
            }
            else
            {
                playerSpeed = _playerSpeed;
            }
        }


        if (inputValue < 0 && !flip)
        {
            //Debug.Log(inputValue);

            yRotation += rotationSpeed * Time.deltaTime;
            LastKey = 1;

            if (yRotation >= 90)
            {
                yRotation = 90;
                flip = true;
            }
        }
        else if (inputValue > 0 && flip)
        {
            yRotation -= rotationSpeed * Time.deltaTime;
            LastKey = 2;

            if (yRotation <= -90)
            {
                yRotation = -90;
                flip = false;
            }
        }
        else
        {
            if (LastKey == 1 && yRotation < 90)
            {
                yRotation += rotationSpeed * Time.deltaTime;
                if (yRotation >= 90)
                {
                    yRotation = 90;
                    flip = true;
                }
            }
            else if (LastKey == 2 && yRotation > -90)
            {
                yRotation -= rotationSpeed * Time.deltaTime;

                if (yRotation <= -90)
                {
                    yRotation = -90;
                    flip = false;
                }
            }
        }
        rotation = Quaternion.Euler(0, yRotation, 0);
        box.transform.rotation = rotation;
        this.transform.position += (move * Time.deltaTime * playerSpeed);
    }

    private void OnCollisionEnter(Collision other)
    {
        canJump = true;
    }
    private void OnCollisionExit(Collision other)
    {
        canJump = false;
    }

    private void OnCollisionStay(Collision collision)
    {
        canJump = true;
    }

    public void IsASide(bool _isASide)
    {
        ASide = _isASide;


        transform.position = new Vector3(transform.position.x, transform.position.y, z);
        z = -z;
    }

    public void PlayerCanMove(bool _canMove)
    {
        canMove = _canMove;
    }

    public void SetPos(float distance)
    {

    }
}
