using System.Collections;
using System.Collections.Generic;
using ReadyPlayerMe.NetcodeSupport;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

public class JoinRoom : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //public void joinRoom()
    //{
    //    // Get the Player object that owns the button press
    //    if (NetworkManager.Singleton.LocalClient.PlayerObject != null)
    //    {
    //        NetworkObject player = NetworkManager.Singleton.LocalClient.PlayerObject;
    //        player.transform.position = new Vector3(8.2f, 0.23f, 3.92f);

    //    }
    //}
    public void joinRoom()
    {
        if (NetworkManager.Singleton.LocalClient.PlayerObject != null)
        {
            //NetworkTransform player = NetworkManager.Singleton.LocalClient.PlayerObject.gameObject.GetComponent<NetworkTransform>();
            //player.transform.position = new Vector3(8.2f, 0.23f, 3.92f);

            GameObject player = NetworkManager.Singleton.LocalClient.PlayerObject.gameObject;
            player.transform.position = new Vector3(8.2f, 0.23f, 3.92f);

        }
        RpcUpdatePositionServerRpc();
    }

    //Gets called by the server and run on the client to update the position
    //[ClientRpc]
    [ServerRpc(RequireOwnership = false)]
    void RpcUpdatePositionServerRpc()
    {
        //NetworkObject player = NetworkManager.Singleton.LocalClient.PlayerObject;
        //player.transform.position = new Vector3(8.2f, 0.23f, 3.92f);
        //NetworkTransform player = NetworkManager.Singleton.LocalClient.PlayerObject.gameObject.GetComponent<NetworkTransform>();
        //player.transform.position = new Vector3(8.2f, 0.23f, 3.92f);
        GameObject player = NetworkManager.Singleton.LocalClient.PlayerObject.gameObject;
        player.transform.position = new Vector3(8.2f, 0.23f, 3.92f);
    }
}
