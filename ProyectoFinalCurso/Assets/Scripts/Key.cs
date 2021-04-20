using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    GameManager gameManager;

    public float angularSpeed;
    public float amplitude;

    Vector3 initPos;
    float timer;

    private void Start()
    {
        initPos = transform.position;
        timer = 0;
        initPos = transform.position;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        float posY = amplitude * Mathf.Sin(angularSpeed * timer);
        transform.position = new Vector3(transform.position.x, initPos.y + posY, transform.position.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        gameManager.OnKeyCollected();
    }
}
