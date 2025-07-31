using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSpaceCanvas : MonoBehaviour
{
    public Transform camera;
    private void Update()
    {
        gameObject.transform.LookAt(camera);
    }
}
