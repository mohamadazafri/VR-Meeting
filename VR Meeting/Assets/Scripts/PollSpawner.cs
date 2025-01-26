using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PollSpawner : NetworkBehaviour
{
    [SerializeField] private GameObject pollPanel;

    private bool hasSpawned = false;
    private GameObject m_PrefabInstance;
    private NetworkObject SpawnedNetworkPollPanel;
    public bool DestroyWithSpawner;

    public void spawnPoll()
    {
        var instance = Instantiate(pollPanel);
        var instanceNetworkObject = instance.GetComponent<NetworkObject>();
        instanceNetworkObject.Spawn();

    }

    public override void OnNetworkDespawn()
    {
        if (IsServer && DestroyWithSpawner && SpawnedNetworkPollPanel != null && SpawnedNetworkPollPanel.IsSpawned)
        {
            SpawnedNetworkPollPanel.Despawn();
        }
        base.OnNetworkDespawn();
    }
}
