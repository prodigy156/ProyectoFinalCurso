using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Transform playerObject, camObject;

    CharacterMovement player;
    CamRotator cam;

    public bool aSide = true;

    bool onPortal = false;

    void Start()
    {
        player = playerObject.gameObject.GetComponent<CharacterMovement>();
        cam = camObject.gameObject.GetComponent<CamRotator>();
    }

    // Update is called once per frame
    void Update()
    {
        bool wantToRotate = Input.GetKey(KeyCode.F);

        if (wantToRotate && cam.IsStopped() && onPortal)
        {
            cam.RotateCam();
            cam.IsSideA(aSide);
            aSide = !aSide;
            player.IsASide();
        }
        player.CanPlayerMove(cam.IsStopped());
    }

    public void OnPortal(bool _onPortal)
    {
        onPortal = _onPortal;
    }
}
