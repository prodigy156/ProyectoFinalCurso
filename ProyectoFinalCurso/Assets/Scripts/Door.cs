using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Transform gameManagerObject;
    GameManager gameManager;

    GameObject keyLock;
    Rigidbody rbLock;
    BoxCollider bcLock;

    bool hasKey = false;
    bool gotKey = false;

    private void Awake()
    {
        gameManager = gameManagerObject.GetComponent<GameManager>();
        rbLock = GetComponentInChildren<Rigidbody>();
        bcLock = GetComponentInChildren<BoxCollider>();
        //keyLock = GetComponentInChildren<GameObject>();
    }

    // Start is called before the first frame update
    void Start()
    {
        rbLock.useGravity = false;
        rbLock.detectCollisions = false;
    }

    public void HasKey()
    {
        hasKey = true;

        rbLock.useGravity = false;
        rbLock.detectCollisions = false;
    }

    public void PlayerGotKey()
    {
        if (!hasKey)
            Debug.LogError("The door did not know there was a key. GameManager has to tell the door with the funciton Door.HasKey(bool)");
        else
        {
            rbLock.useGravity = true;
            rbLock.detectCollisions = true;

            rbLock.AddTorque(transform.right * -100, ForceMode.Force);
            Invoke("DestroyLock", 3);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!hasKey || hasKey && !gotKey)
        {
            gameManager.PlayerMightExit(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        gameManager.PlayerMightExit(false);
    }

    private void DestroyLock()
    {
        rbLock.gameObject.SetActive(false);
    }
}
