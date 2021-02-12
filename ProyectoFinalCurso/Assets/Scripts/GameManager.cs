using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Transform playerObject, camObject;
    public Transform[] cubes;

    PlayerController player;
    CamRotator cam;
    Box[] cube;

    public bool aSide = true;

    bool onPortal = false;

    // Start is called before the first frame update
    void Start()
    {
        player = playerObject.gameObject.GetComponent<PlayerController>();
        cam = camObject.gameObject.GetComponent<CamRotator>();

        cube = new Box[cubes.Length];

        for (int i = 0; i < cubes.Length; i++)
        {
            cube[i] = cubes[i].GetComponent<Box>();
        }
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
            for (int i = 0; i < cubes.Length; i++)
            {
                cube[i].TellSide(aSide);
            }
        }

        player.PlayerCanMove(cam.IsStopped());

    }

    public void OnPortal(bool _onPortal)
    {
        onPortal = _onPortal;
    }
}
