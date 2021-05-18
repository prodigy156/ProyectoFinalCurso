using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float distance = 0.5f;
    CharacterMovement player;
    CamRotator cam;
    Door door;

    GameObject[] objects2P;

    public bool aSide = true;

    bool onPortal = false;

    bool key = false;
    bool mightExit = false;
    bool playerGotTheKey = false;

    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterMovement>();
        cam = GameObject.FindGameObjectWithTag("CamRotator").GetComponent<CamRotator>();
        door = GameObject.FindGameObjectWithTag("Door").GetComponent<Door>();

        GameObject[] box2p = GameObject.FindGameObjectsWithTag("2P-Box");
        GameObject[] ground2p = GameObject.FindGameObjectsWithTag("2P-Ground");
        objects2P = new GameObject[box2p.Length + ground2p.Length];

        for (int i = 0; i < box2p.Length; i++)
        {
            objects2P[i] = box2p[i];
        }
        if (ground2p.Length > 0)
        {
            int j = 0;
            for (int i = box2p.Length - 1; i < ground2p.Length; i++)
            {
                objects2P[i] = ground2p[j];
                j++;
            }
        }
    }

    private void Start()
    {
        
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
            //player.IsASide(aSide);

            for (int i = 0; i < objects2P.Length; i++)
            {
                if (aSide)
                    objects2P[i].SendMessage("SetPos", -distance);
                else
                    objects2P[i].SendMessage("SetPos", distance);
            }

            if (aSide)
                player.SetPos(-distance);
            else
                player.SetPos(distance);
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

        player.CanPlayerMove(cam.IsStopped());
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
