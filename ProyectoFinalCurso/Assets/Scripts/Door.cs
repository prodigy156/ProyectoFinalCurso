using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    GameManager gameManager;

    public Rigidbody rbLock;

    bool hasKey = false;
    bool gotKey = false;

    private void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    public void HasKey()
    {
        hasKey = true;

        rbLock.gameObject.SetActive(true);

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
