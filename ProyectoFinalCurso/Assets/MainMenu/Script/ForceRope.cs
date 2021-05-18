﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceRope : MonoBehaviour
{
    [SerializeField]
    private Vector2 force;


    [ContextMenu("ApplyForce")]
    private void ApplyForceToJoint()
    {
        Rigidbody rb = GetComponent<Rigidbody>();

        if (rb != null)
            rb.AddForce(force);
        else
        {
            Rigidbody2D rb2D = GetComponent<Rigidbody2D>();
            if (rb2D != null)
                rb2D.AddForce(force);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
            ApplyForceToJoint();
    }
}
