using System.Collections.Generic;
using Unity.Netcode;

public class RelayPlayerIterator : PlayerIterator<NetworkClient>
{
    private List<ulong> playerList = new List<ulong>();
    private int currentIndex = 0;

    public RelayPlayerIterator()
    {
        foreach (var client in NetworkManager.Singleton.ConnectedClients)
        {
            playerList.Add(client.Key); // Store player IDs
        }
    }

    public override bool hasNext()
    {
        return currentIndex < playerList.Count;
    }

    public override NetworkClient getNext()
    {
        if (!hasNext()) return null; // No more players

        ulong playerId = playerList[currentIndex];
        currentIndex++;

        return NetworkManager.Singleton.ConnectedClients.ContainsKey(playerId)
            ? NetworkManager.Singleton.ConnectedClients[playerId]
            : null;
    }

    public override void reset()
    {
        currentIndex = 0; // Reset iteration
    }
}
