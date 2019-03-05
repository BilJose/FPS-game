using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.Match;
using UnityEngine.Networking;

public class PauseMenu : MonoBehaviour
{
    public static bool IsOn = false;

    private NetworkManager networkManager;


    void start()
    {
        
        networkManager = NetworkManager.singleton;
    }
    public void LeaveRoom()
    {
        MatchInfo match = networkManager.matchInfo;
        networkManager.matchMaker.DropConnection(match.networkId, match.nodeId, 0, networkManager.OnDropConnection);
        networkManager.StopHost();

    }

}
