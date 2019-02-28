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
        MatchInfo matchInfo = networkManager.matchInfo;
        networkManager.matchMaker.DropConnection(matchInfo.networkId, matchInfo.nodeId, 0, networkManager.OnDropConnection);
        networkManager.StopHost();

    }

}
