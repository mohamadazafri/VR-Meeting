using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class TeleportPlayer : MonoBehaviour
    
{
    public GameObject player;
    public TMP_InputField numberOfGroups;
    public TMP_InputField numberOfParticipants;
    public List<GameObject> allPlayer = new List<GameObject>();
    private float positionOffset = 0.1f;
    private List<Vector3> groupSpawnPositions = new List<Vector3>

    {
        new Vector3(8.2f, 0.23f, 3.92f),
        new Vector3(8.2f, 0.23f, -5f),    // Group 2 spawn position
        new Vector3(-8.2f, 0.23f, 3.92f),  // Group 3 spawn position
        new Vector3(-8.2f, 0.23f, -5f)    // Group 4 spawn position
    };

    // Start is called before the first frame update
    void Start()
    {
        GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag("Player");

        // Add them to the list
        //allPlayer.AddRange(objectsWithTag);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Teleport()
    {
        var listPlayer = NetworkManager.Singleton.ConnectedClientsList;
        Debug.Log(NetworkManager.Singleton.ConnectedClientsList);

        List<NetworkObject> clientPlayers = new List<NetworkObject>();

        for(int i = 0; i < NetworkManager.Singleton.ConnectedClientsList.Count; i++)
        {
            //if (listPlayer[i].ClientId != 0)
            //{
                clientPlayers.Add(listPlayer[i].PlayerObject);
            //}
        }


        //Debug.Log(numberOfGroups.text);
        //Debug.Log(numberOfParticipants.text);
        //player.transform.position = new Vector3(8.2f, 0.23f, 3.92f);

        //foreach (GameObject obj in allPlayer)
        //{
        //    Debug.Log(obj.name);
        //}

        AssignParticipantsToGroups(clientPlayers);
    }

    void AssignParticipantsToGroups(List<NetworkObject> participantList)
    {
        int groupIndex = 0;
        int numOfGroups = 4;
        int numOfParticipants = 5;
        float cumulativeOffset = 0; // Track cumulative offset for each group

        foreach (NetworkObject participant in participantList)
        {
            // Calculate group index (round-robin assignment)
            groupIndex = groupIndex % numOfGroups;

            // Get the base spawn position for the group
            Vector3 spawnPosition = groupSpawnPositions[groupIndex];

            // Add cumulative offset to the position (slightly move right for each participant)
            spawnPosition.x += cumulativeOffset;

            // Teleport participant to the adjusted position
            TeleportParticipant(participant, spawnPosition);

            // Increment offset for the next participant
            cumulativeOffset += positionOffset;

            // Move to the next group
            groupIndex++;
        }
    }

    void TeleportParticipant(NetworkObject participant, Vector3 spawnPosition)
    {
        // Move participant to the spawn position

        participant.transform.position = spawnPosition;
        Debug.Log($"{participant.name} teleported to {spawnPosition}");
    }

}
