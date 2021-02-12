using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamRotator : MonoBehaviour
{
    public float rotationTime = 1.0f;

    public Transform rot1, rot2;
    Transform rot, nextRot;

    bool isASide = true;

    enum States { ROTATING, STOP }
    States state, nextState;

    float timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        state = nextState = States.STOP;
    }

    // Update is called once per frame
    void Update()
    {
        if (state != nextState)
        {
            switch (nextState)
            {
                case States.STOP:
                    break;
                case States.ROTATING:
                    timer = 0;
                    if(isASide)
                    {
                        rot = rot1;
                        nextRot = rot2;
                    }
                    else
                    {
                        rot = rot2;
                        nextRot = rot1;
                    }
                    isASide = !isASide;
                    break;
                default:
                    break;
            }
            state = nextState;
        }

        switch (state)
        {
            case States.STOP:
                break;
            case States.ROTATING:

                timer += Time.deltaTime;

                if (timer > rotationTime) { timer = rotationTime; }

                float interpolationFactor = timer / rotationTime;

                transform.rotation = Quaternion.Slerp(rot.rotation, nextRot.rotation, interpolationFactor);

                if (timer >= rotationTime)
                {
                    transform.rotation = nextRot.rotation;
                    nextState = States.STOP;
                }

                break;
            default:
                break;
        }
    }

    public void RotateCam()
    {
        if (state == States.STOP)
            nextState = States.ROTATING;
    }

    public bool IsStopped()
    {
        return (state == States.STOP);
    }

    public void IsSideA(bool _isASide)
    {
        isASide = _isASide;
    }
}
