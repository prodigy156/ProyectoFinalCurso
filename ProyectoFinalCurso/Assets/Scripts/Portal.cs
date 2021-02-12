using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public Transform gameManagerObject;
    GameManager gameManager;

    private void Start()
    {
        gameManager = gameManagerObject.gameObject.GetComponent<GameManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            gameManager.OnPortal(true);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            gameManager.OnPortal(false);
        }
    }
}
