using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchController : MonoBehaviour
{
    public FixedTouchField _FixedTouchField;
    public CameraLook _CameraLook;
    //public PlayerMove _PlayerMove;

    public FixedButtonFire _FixedButtonFire;
    public PlayerFire _PlayerFire;

    void Start()
    {

    }


    void Update()
    {
        _CameraLook.LockAxis = _FixedTouchField.TouchDist;
        _PlayerFire.isPassed = _FixedButtonFire.FirePressed;
    }
}
