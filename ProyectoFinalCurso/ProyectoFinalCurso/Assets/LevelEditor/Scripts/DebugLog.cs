﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugLog : MonoBehaviour
{
    int numero = 3;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("hello" + numero);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward);
    }
}
