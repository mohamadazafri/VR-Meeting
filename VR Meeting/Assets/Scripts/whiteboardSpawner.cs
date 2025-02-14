using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class WhiteboardSpawner : NetworkBehaviour
{
    [SerializeField] private GameObject whiteboardMain; 
    [SerializeField] private GameObject whiteboardBreakoutRoom1;
    [SerializeField] private GameObject whiteboardBreakoutRoom2;
    [SerializeField] private GameObject whiteboardBreakoutRoom3;
    [SerializeField] private GameObject whiteboardBreakoutRoom4;

    private NetworkObject m_SpawnedNetworkWhiteboard;
    public bool DestroyWithSpawner;

    public void spawnAllWhiteboard()
    {
  
            spawnMainWhiteboard();
            spawnWhiteboardBreakoutRoom1();
            spawnWhiteboardBreakoutRoom2();
            spawnWhiteboardBreakoutRoom3();
            spawnWhiteboardBreakoutRoom4();

    }

    public void spawnMainWhiteboard()
    {
        var instance = Instantiate(whiteboardMain);
        var instanceNetworkObject = instance.GetComponent<NetworkObject>();
        instanceNetworkObject.Spawn();
        
    }
    public void spawnWhiteboardBreakoutRoom1()
    {
        var instance = Instantiate(whiteboardBreakoutRoom1);
        var instanceNetworkObject = instance.GetComponent<NetworkObject>();
        instanceNetworkObject.Spawn();
    }
    public void spawnWhiteboardBreakoutRoom2()
    {
        var instance = Instantiate(whiteboardBreakoutRoom2);
        var instanceNetworkObject = instance.GetComponent<NetworkObject>();
        instanceNetworkObject.Spawn();
    }
    public void spawnWhiteboardBreakoutRoom3()
    {
        var instance = Instantiate(whiteboardBreakoutRoom3);
        var instanceNetworkObject = instance.GetComponent<NetworkObject>();
        instanceNetworkObject.Spawn();
    }
    public void spawnWhiteboardBreakoutRoom4()
    {
        var instance = Instantiate(whiteboardBreakoutRoom4);
        var instanceNetworkObject = instance.GetComponent<NetworkObject>();
        instanceNetworkObject.Spawn();
    }
    

    public override void OnNetworkDespawn()
    {
        if (IsServer && DestroyWithSpawner && m_SpawnedNetworkWhiteboard != null && m_SpawnedNetworkWhiteboard.IsSpawned)
        {
            m_SpawnedNetworkWhiteboard.Despawn();
        }
        base.OnNetworkDespawn();
    }
}
