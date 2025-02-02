using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerFire : MonoBehaviour
{
    public Transform Camera;
    public bool isPassed;
    RaycastHit hit;
    public float Range = 200f;
    public LayerMask layerMask;
    private TMP_InputField currentInputField;
    public GameObject networkManager;

    private NetworkConnect networkConnect;
    private GetJoinCode getJoinCode;
    private float LastTime;

    private void Awake()
    {
        networkConnect = networkManager.GetComponent<NetworkConnect>();
        getJoinCode = networkManager.GetComponent<GetJoinCode>();
    }
    void Start()
    {

    }

    void Update()
    {
        if (isPassed && Time.time > LastTime + 0.15f) 
        {
            shoot();
            LastTime= Time.time;
        }
    }
    public void shoot()
    {
        if (Physics.Raycast(Camera.position , Camera.forward, out hit , Range, layerMask))
        { 
            Debug.Log(hit.transform.name);

            TMP_InputField inputField = hit.collider.GetComponent<TMP_InputField>();
            if (inputField != null)
            {
                ShowKeyboard(inputField);
            }

            Button button = hit.collider.GetComponent<Button>();
            if (button != null)
            {
                button.onClick.Invoke(); // Simulate button click
            }

            TMP_Dropdown dropdown = hit.collider.GetComponent<TMP_Dropdown>();
            if (dropdown != null)
            {
                dropdown.Show(); // Open the TMP_Dropdown
            }
            //switch (hit.transform.name)
            //{
            //    case "SubmitLogin":
            //        networkConnect.SignIn();
            //        break;
            //    case "SubmitRegister":
            //        networkConnect.Register();
            //        break;   
            //    case "JoinSubmit":
            //        getJoinCode.TransferDataToNetworkManager();
            //        break;       
            //    case "JoinSubmit":
            //        getJoinCode.TransferDataToNetworkManager();
            //        break;

            //    default:
            //        break;
            //}
        }
    }

    void ShowKeyboard(TMP_InputField inputField)
    {
        currentInputField = inputField;
        inputField.Select();
        inputField.ActivateInputField();
        //TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default);
    }
}
