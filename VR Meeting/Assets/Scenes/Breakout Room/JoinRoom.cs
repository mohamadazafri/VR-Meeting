using System.Collections;
using System.Collections.Generic;
using ReadyPlayerMe.NetcodeSupport;
using Unity.Netcode;
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
            ClientNetworkTransform player = NetworkManager.Singleton.LocalClient.PlayerObject.gameObject.GetComponent<ClientNetworkTransform>();
            player.transform.position = new Vector3(8.2f, 0.23f, 3.92f);

        }
        RpcUpdatePositionClientRpc();
    }

    //Gets called by the server and run on the client to update the position
    [ClientRpc]
    void RpcUpdatePositionClientRpc()
    {
        NetworkObject player = NetworkManager.Singleton.LocalClient.PlayerObject;
        player.transform.position = new Vector3(8.2f, 0.23f, 3.92f);
    }
}
