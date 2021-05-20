using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    CharacterController controller;
    CharacterStates characterStates;

    //character public atributes
    public Transform box;

    public float speed = 6f;
    public float turnSpeed = 1000f;
    public float gravity = -9.81f;

    //character private atributes
    private bool lookingRight = true;

    //Gravity - GroundCheck
        //Public
    public Transform groundCheck;
    public float groundDistance = 0.1f;
    public LayerMask groundMask;
    public float jumpHeight = 3f;
        //Private
    Vector3 velocity; //vertical downwards velocity
    bool isGrounded, jumping = false;
    //World Rotation
    bool isASide = true;
    bool canMove = true;

    enum State
    {
        STOPPED, WALKING_RIGHT, WALKING_LEFT, JUMPING_RIGHT, JUMPING_LEFT
    }
    State state, nextState;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        characterStates = box.GetComponent<CharacterStates>();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(groundCheck.position, new Vector3(0.65f, groundDistance, 0.1f));
    }

    private void Update()
    {
        if (state != nextState)
        {
            switch (nextState)
            {
                case State.STOPPED:
                    characterStates.ChangeState(CharacterStates.State.IDLE, CharacterStates.Direction.RIGHT);
                    break;
                case State.WALKING_RIGHT:
                    characterStates.ChangeState(CharacterStates.State.RUNNING, CharacterStates.Direction.RIGHT);
                    break;
                case State.WALKING_LEFT:
                    characterStates.ChangeState(CharacterStates.State.RUNNING, CharacterStates.Direction.LEFT);
                    break;
                case State.JUMPING_RIGHT:
                    characterStates.ChangeState(CharacterStates.State.JUMPING, CharacterStates.Direction.RIGHT);
                    break;
                case State.JUMPING_LEFT:
                    characterStates.ChangeState(CharacterStates.State.JUMPING, CharacterStates.Direction.LEFT);
                    break;
            }
            state = nextState;
        }

        float horizontal = Input.GetAxisRaw("Horizontal");
        Vector3 direction = new Vector3(horizontal, 0f, 0f).normalized;
        if(!isASide)
        {
            direction *= -1;
        }
        if(direction.magnitude >= 0.1f && canMove)
        {
            if(horizontal < 0f)//L
            {
                lookingRight = false;
                nextState = State.WALKING_LEFT;
            }
            else if(horizontal > 0f)//R
            {
                lookingRight = true;
                nextState = State.WALKING_RIGHT;
            }

            controller.Move(direction * speed * Time.deltaTime);
        }
        else
        {
            if (isGrounded)
                nextState = State.STOPPED;
        }
        
        //Jump
        {
            if (Input.GetButtonDown("Jump") && isGrounded && canMove)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
                jumping = true;

                if (horizontal > 0f)
                {
                    nextState = State.JUMPING_RIGHT;
                }
                else
                {
                    nextState = State.JUMPING_LEFT;
                }

                lookingRight = !lookingRight;
            }
        }

        if (!isGrounded)
        {
            if (horizontal > 0f)
            {
                nextState = State.JUMPING_RIGHT;
            }
            else
            {
                nextState = State.JUMPING_LEFT;
            }
        }

        //gravity
        {
            isGrounded = Physics.CheckBox(groundCheck.position, new Vector3(0.65f, groundDistance, 0.1f),  new Quaternion(0,0,0,0), groundMask);

            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -0f;
                if (jumping)
                {
                    jumping = false;

                    nextState = State.STOPPED;

                    lookingRight = !lookingRight;
                }
            }
            else
                isGrounded = false;

            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }
    }

     /// <summary>
    /// OnControllerColliderHit is called when the controller hits a
    /// collider while performing a Move.
    /// </summary>
    /// <param name="hit">The ControllerColliderHit data associated with this collision.</param>
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;

        //no rigidbody
        if(body == null || body.isKinematic)
        {
            return;
        }

        // We dont want to push objects below us
        if (hit.moveDirection.y < -0.3)
        {
            return;
        }

        // Calculate push direction from move direction,
        // we only push objects to the sides never up and down
        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, 0);

        // If you know how fast your character is trying to move,
        // then you can also multiply the push velocity by that.

        // Apply the push
        body.velocity = pushDir.normalized * speed/2;
        Debug.Log(body.velocity);
    }

    //checks if character is on A side or B side on the map
    public void SetPos(float distance)
    {
        isASide = !isASide;
        controller.Move(new Vector3(0, 0, distance * 2));
        characterStates.ChangeSide(isASide);
    }

    public void CanPlayerMove(bool _canMove)
    {
        canMove = _canMove;
    }
}
