using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Vivox;

public class VivoxPlayerIterator : PlayerIterator<VivoxParticipant>
{
    List<VivoxParticipant> playerList;
    private int currentIndex = 0;
      
    public VivoxPlayerIterator()
    {
        playerList = new List<VivoxParticipant>();

        foreach (var channel in VivoxService.Instance.ActiveChannels["vrMeetChannel"])
        {
            VivoxParticipant participants = channel; // Get participants in this channel
            playerList.Add(participants); // Add them to the list
        }

    }

    public override bool hasNext()
    {
        return currentIndex < playerList.Count;
    }

    public override VivoxParticipant getNext()
    {
        if (!hasNext()) return null; // No more players

        return playerList[currentIndex++];
    }

    public override void reset()
    {
        currentIndex = 0; // Reset iteration
    }
}
