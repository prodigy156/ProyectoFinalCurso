using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStates : MonoBehaviour
{
    public GameObject idlePrefab, runningRightPrefab, runningLeftPrefab, jumpingRightPrefab, jumpingLeftPrefab;
    GameObject idle, runningRight, runningLeft, jumpingRight, jumpingLeft;
    GameObject currentState, beforeState;
    [SerializeField]
    float distanceFromThePoint;

    float yRotation;
    public float turnSpeed = 1000f;

    Vector3 aPosition, bPosition;
    Quaternion aRotation = Quaternion.Euler(0, 180, 0);
    Quaternion bRotation = Quaternion.Euler(0, 0, 0);

    public enum Direction
    {
        RIGHT, LEFT
    }
    Direction currentDirection, nextDirection;
    public enum State
    {
        IDLE, RUNNING, JUMPING
    }
    State state, nextState;

    Matrix4x4 matrixA, matrixB;

    bool wantsToChange = false;
    bool controlWhatSideHasToChange;
    bool rotating = false;
    bool rotatingToA = false, rotatingToB = false;

    private void Awake()
    {
        idle = Instantiate(idlePrefab, transform);
        runningRight = Instantiate(runningRightPrefab, transform);
        runningLeft = Instantiate(runningLeftPrefab, transform);
        jumpingRight = Instantiate(jumpingRightPrefab, transform);
        jumpingLeft = Instantiate(jumpingLeftPrefab, transform);

        idle.SetActive(false);
        runningRight.SetActive(false);
        runningLeft.SetActive(false);
        jumpingRight.SetActive(false);
        jumpingLeft.SetActive(false);

        beforeState = runningRight;
        runningRight.SetActive(true);

        currentState = idle;
        idle.SetActive(true);

        yRotation = transform.eulerAngles.y;
    }

    private void Start()
    {
        state = nextState = State.IDLE;
        controlWhatSideHasToChange = true;
        wantsToChange = false;

        aPosition = new Vector3(distanceFromThePoint, 0, 0);
        bPosition = new Vector3(-distanceFromThePoint, 0, 0);
    }

    private void Update()
    {
        if (wantsToChange)
        {
            GameObject nextObjectState;
            switch (nextState)
            {
                case State.RUNNING:
                    if (nextDirection == Direction.RIGHT)
                    {
                        nextObjectState = runningRight;
                    }
                    else //if (nextDirection == Direction.LEFT)
                    {
                        nextObjectState = runningLeft;
                    }
                    break;
                case State.JUMPING:
                    if (nextDirection == Direction.RIGHT)
                    {
                        nextObjectState = jumpingRight;
                    }
                    else //if (nextDirection == Direction.LEFT)
                    {
                        nextObjectState = jumpingLeft;
                    }
                    break;
                case State.IDLE:
                    nextObjectState = idle;
                    break;
                default:
                    nextObjectState = idle;
                    break;
            }

            beforeState.SetActive(false);
            beforeState = currentState;
            currentState = nextObjectState;
            nextObjectState.SetActive(true);

            rotating = true;
            if (controlWhatSideHasToChange)
            {
                nextObjectState.transform.position = transform.localToWorldMatrix.MultiplyPoint(bPosition);
                nextObjectState.transform.rotation = bRotation;
                rotatingToB = true;
                controlWhatSideHasToChange = false;
            }
            else
            {
                nextObjectState.transform.position = transform.localToWorldMatrix.MultiplyPoint(aPosition);
                nextObjectState.transform.rotation = aRotation;
                rotatingToA = true;
                controlWhatSideHasToChange = true;
            }
            wantsToChange = false;
        }

        if (rotating)
        {
            if (rotatingToA)
            {
                yRotation += turnSpeed * Time.deltaTime;

                if (yRotation > 90)
                {
                    yRotation = 90;
                    rotating = rotatingToA = false;
                }
            }
            else if (rotatingToB)
            {
                yRotation -= turnSpeed * Time.deltaTime;

                if (yRotation < -90f)
                {
                    yRotation = -90f;
                    rotating = rotatingToB = false;
                }
            }

            transform.rotation = Quaternion.Euler(new Vector3(0f, yRotation, 0f));
        }
    }

    /// <summary>
    /// Cambia el mesh de "la parte de atrás" del jugador
    /// </summary>
    /// <param name="_nextState"></param>
    public void ChangeState(State _nextState, Direction _nextDirection)
    {
        nextState = _nextState;
        nextDirection = _nextDirection;
        wantsToChange = true;
    }

    public void ChangeSide()
    {

    }
}