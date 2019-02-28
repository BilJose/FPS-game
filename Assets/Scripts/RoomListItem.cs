﻿using UnityEngine.Networking.Match;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomListItem : MonoBehaviour
{

    public delegate void JoinRoomDelegate(MatchInfoSnapshot _match);

    private JoinRoomDelegate joinRoomCallback;

    [SerializeField]
    private Text roomnameText;



    private MatchInfoSnapshot match;



    public void Setup(MatchInfoSnapshot _match, JoinRoomDelegate _joinRoomCallback )
    {
        match = _match;
        joinRoomCallback = _joinRoomCallback;
        roomnameText.text = match.name + "(" + match.currentSize + "/" + match.maxSize + ")";
        
    }

    public void JoinRoom()
    {
        joinRoomCallback.Invoke(match);
    }
}
