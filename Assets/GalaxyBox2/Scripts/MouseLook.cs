using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//[AddComponentMenu("Camera-Control/Smooth Mouse Look")]
public class SmoothMouseLook : MonoBehaviour
{
    Vector3 mouseAccel = Vector3.zero;
    void Update()
    {
        float theta = 0.0f;
        float phi = 0.0f;
        float s = 1.0f;
        if (Input.GetMouseButton(1))
        {
            theta = s * Input.GetAxis("Mouse X");
            phi = s * Input.GetAxis("Mouse Y") * -1.0f;
        }
        mouseAccel += new Vector3(theta, phi, 0);
        mouseAccel *= 0.9f;
        Transform t = transform;
        t.RotateAround(t.position, t.up, mouseAccel.x);
        t.RotateAround(t.position, t.right, mouseAccel.y);



    }

    void Start()
    {
    }

    }