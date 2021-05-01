using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Transform playerObject, camObject, doorObject;

    PlayerController player;
    CamRotator cam;
    Door door;

    public bool aSide = true;

    bool onPortal = false;

    bool key = false;
    bool mightExit = false;
    bool playerGotTheKey = false;

    // Start is called before the first frame update
    void Awake()
    {
        player = playerObject.GetComponent<PlayerController>();
        cam = camObject.GetComponent<CamRotator>();
        door = doorObject.GetComponent<Door>();
    }

    // Update is called once per frame
    void Update()
    {
        bool wantsToDoAction = Input.GetKey(KeyCode.UpArrow);

        if (wantsToDoAction && cam.IsStopped() && onPortal)
        {
            cam.RotateCam();
            cam.IsSideA(aSide);
            aSide = !aSide;
            player.IsASide(aSide);
        }
        else if (wantsToDoAction && mightExit)
        {
            if (key)
            {
                if (playerGotTheKey)
                {
                    Debug.Log("Next level");
                }
                else
                {
                    Debug.Log("Key needed");
                }
            }
            else
            {
                Debug.Log("Next level");
            }
        }

        player.PlayerCanMove(cam.IsStopped());
    }

    public void OnPortal(bool _onPortal)
    {
        onPortal = _onPortal;
    }

    public void OnKeyCollected()
    {
        playerGotTheKey = true;
        door.PlayerGotKey(); 
    }

    public void ThereIsAKey()
    {
        key = true;
        door.HasKey();
    }

    public void PlayerMightExit(bool _mightExit)
    {
        mightExit = _mightExit;
    }
}
