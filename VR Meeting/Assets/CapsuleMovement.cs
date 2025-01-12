using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapsuleMovement : MonoBehaviour
{

    // Update is called once per frame
    public Transform xrOrigin; // Reference to XR Origin
    public Transform xrCamera; // Reference to XR Camera (child of XR Origin)
    public Camera mainCamera;

    private void Awake()
    {
        xrOrigin = GameObject.FindGameObjectWithTag("XROrigin").transform;
        xrCamera = GameObject.FindGameObjectWithTag("MainCamera").transform;
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }
    // Start is called before the first frame update
    void Start()
    {   
        
        //xrOrigin = GameObject.FindGameObjectWithTag("XROrigin").transform;
        //xrCamera = GameObject.FindGameObjectWithTag("MainCamera").transform;

        //mainCamera.enabled = true; 
    }
    void Update()
    {
        if (xrOrigin && xrCamera)
        {
            // Sync the position of the capsule with the XR Origin
            Vector3 position = xrOrigin.position;
            position.y = xrCamera.position.y; // Align with camera height
            //transform.position = position;
            transform.position = xrCamera.position;

        }
    }
}
