using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Transform playerObject, camObject;

    PlayerController player;
    CamRotator cam;

    public bool aSide = true;

    bool onPortal = false;

    // Start is called before the first frame update
    void Start()
    {
        player = playerObject.gameObject.GetComponent<PlayerController>();
        cam = camObject.gameObject.GetComponent<CamRotator>();
    }

    // Update is called once per frame
    void Update()
    {
        bool wantToRotate = Input.GetKey(KeyCode.UpArrow);

        if (onPortal)
        {
            int i = 0;
        }

        if (wantToRotate && cam.IsStopped() && onPortal)
        {
            cam.RotateCam();
            cam.IsSideA(aSide);
            aSide = !aSide;
            player.IsASide(aSide);
        }

        //player.PlayerCanMove(cam.IsStopped());

    }

    public void OnPortal(bool _onPortal)
    {
        onPortal = _onPortal;
    }
}
