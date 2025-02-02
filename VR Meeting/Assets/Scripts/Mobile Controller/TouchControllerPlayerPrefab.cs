using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchControllerPlayerPrefab : MonoBehaviour
{
    [SerializeField] private GameObject mobileController;
    [SerializeField] private GameObject mobileControllerButtons;
    [SerializeField] private GameObject mobileControllerCrosshair;
    public FixedTouchField _FixedTouchField;
    public CameraLook _CameraLook;
    //public CameraLookPlayerPrefab _CameraLook;
    public FixedButtonFire _FixedButtonFire;
    public PlayerPrafabFire _PlayerFire;

    void Start()
    {
        #if (UNITY_ANDROID || UNITY_EDITOR) || __ANDROID__
                mobileController.SetActive(true);
                mobileControllerButtons.SetActive(true);
                mobileControllerCrosshair.SetActive(true);
        #endif
        //GameObject xrOrigin = GameObject.Find("XR Origin"); // Find XR Origin
        //if (xrOrigin != null)
        //{
        //    Transform cameraOffset = xrOrigin.transform.Find("Camera Offset"); // Find Camera Offset
        //    if (cameraOffset != null)
        //    {
        //        Transform mainCamera = cameraOffset.Find("Main Camera"); // Find Main Camera
        //        if (mainCamera != null)
        //        {
        //            _CameraLook = mainCamera.GetComponent<CameraLook>();
        //            //_CameraLook = mainCamera.GetComponent<CameraLookPlayerPrefab>();
        //        }
        //        else
        //        {
        //            Debug.LogError("Main Camera not found inside Camera Offset.");
        //        }
        //    }
        //    else
        //    {
        //        Debug.LogError("Camera Offset not found inside XR Origin.");
        //    }
        //}
        //else
        //{
        //    Debug.LogError("XR Origin not found in the scene.");
        //}
    }


    void Update()
    {
        _CameraLook.LockAxis = _FixedTouchField.TouchDist;
        _PlayerFire.isPassed = _FixedButtonFire.FirePressed;
    }
}
