using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public float speed;

    public float jumpVel;

    bool ASide = true;
    bool canMove;

    Rigidbody rb;

    float z = 0.6f;

    float timer = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (canMove)
        {
            float horizontal = Input.GetAxis("Horizontal");

            if(!ASide)
            {
                horizontal *= -1;
            }

            bool jump = Input.GetKey(KeyCode.Space);

            transform.position += new Vector3(horizontal * speed * Time.deltaTime, 0, 0);

            if (jump)
            {
                if (timer >= 2)
                {
                    rb.AddForce(0, jumpVel, 0);
                    timer = 0;
                }
            }
        }
    }

    public void IsASide(bool _isASide)
    {
        ASide = _isASide;

        
        transform.position = new Vector3(transform.position.x, transform.position.y, z);
        z = -z;
    }

    public void PlayerCanMove(bool _canMove)
    {
        canMove = _canMove;
    }
}
