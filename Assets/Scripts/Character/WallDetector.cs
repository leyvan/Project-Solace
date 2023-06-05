using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallDetector : MonoBehaviour
{
    public bool wallDetected;
    //public Transform currentWallTransform;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Wall")
        {
            wallDetected = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Wall")
        {
            wallDetected = false;
        }
    }
}
