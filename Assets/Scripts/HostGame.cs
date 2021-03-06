﻿using UnityEngine.Networking;
using UnityEngine;

public class HostGame : MonoBehaviour
{
    [SerializeField]
    private uint roomSize = 10;

    private string roomName;


    NetworkManager networkManager;
    void Start()
    {
        networkManager = NetworkManager.singleton;
        if( networkManager.matchMaker == null)
        {
            networkManager.StartMatchMaker();
        }
    }
    public void SetRoomName(string _name)
    {
        roomName = _name;
    }

    public void CreateRoom()
    {
        if(roomName != "" && roomName != null)
        {
            Debug.Log("Creating room " + roomName + "with room for " + roomSize + " players");

            networkManager.matchMaker.CreateMatch(roomName, roomSize, true, "", "", "", 0, 0, networkManager.OnMatchCreate);
        }
    }
}