using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Unity.Netcode;
using UnityEngine;

public class CameraLookPlayerPrefab : MonoBehaviour
{
    private float XMove;
    private float YMove;
    private float XRotation;
    private Transform PlayerBody;
    public Vector2 LockAxis;
    public float Sensitivity = 5f;
    void Start()
    {

    }
    void Update()
    {
        if (PlayerBody != null)
        {
            XMove = LockAxis.x * Sensitivity * Time.deltaTime;
            YMove = LockAxis.y * Sensitivity * Time.deltaTime;
            XRotation -= YMove;
            XRotation = Mathf.Clamp(XRotation, -90f, 90f);

            transform.localRotation = Quaternion.Euler(XRotation,0,0);
            PlayerBody.Rotate(Vector3.up * XMove);

        }else
        {
            try
            {

                NetworkClient hostPlayer = NetworkManager.Singleton.LocalClient;
                GameObject hostPlayerGameObject = hostPlayer.PlayerObject.gameObject;
                PlayerBody = hostPlayerGameObject.transform;
            }
            catch (System.Exception e)
            {
                Debug.Log(e.Message);
                
            }
        }
    }
}
