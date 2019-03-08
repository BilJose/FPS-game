using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine;

public class JoinGame : MonoBehaviour
{


    List<GameObject> roomList = new List<GameObject>();


    [SerializeField]
    private Text status;
    [SerializeField]
    private GameObject roomListItemPrefab;
    [SerializeField]
    Transform roomListParent;

    private NetworkManager networkManager;



    void Start()
    {
        networkManager = NetworkManager.singleton;
        if (networkManager.matchMaker == null)
        {
            networkManager.StartMatchMaker();
        }

        RefreshRoomList();
    }

    public void RefreshRoomList()
    {
        ClearRoomList();

        if(networkManager.matchMaker == null)
        {
            networkManager.StartMatchMaker();
        }
        networkManager.matchMaker.ListMatches(0, 20, "", true, 0, 0, OnMatchList);
        status.text = "Loading...";

    }
    public void OnMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matchList)
    {
        status.text = "";

        if (!success || matchList == null)
        {
            status.text = "Couldn't get room list.";
            return;
        }

        foreach (MatchInfoSnapshot match in matchList)
        {
            GameObject _roomListItemGO = Instantiate(roomListItemPrefab);
            _roomListItemGO.transform.SetParent(roomListParent);


            RoomListItem _roomListItem = _roomListItemGO.GetComponent<RoomListItem>();
            if(_roomListItem != null)
            {
                _roomListItem.Setup(match, JoinRoom);
            }




            roomList.Add(_roomListItemGO);

        }
        if(roomList.Count == 0)
        {
            status.text = "no Rooms at the moment";
        }
    }    
    void ClearRoomList()
    {
        for (int i = 0; i < roomList.Count; i++)
        {
            Destroy(roomList[i]);
        }
        roomList.Clear();
    }
    public void JoinRoom(MatchInfoSnapshot _match)
    {
        networkManager.matchMaker.JoinMatch(_match.networkId, "","","",0,0, networkManager.OnMatchJoined);
        StartCoroutine(WaitForJoin());
        
    }

    IEnumerator WaitForJoin()
    {
        ClearRoomList();
        

        int countDown = 10;
        while (countDown > 0)
        {
            status.text = "Joining... (" + countDown + ")";
            yield return new WaitForSeconds(1);
            countDown--;
        }
        status.text = "Failed to connect.";
        yield return new WaitForSeconds(1);
        MatchInfo match = networkManager.matchInfo;
        if (match != null)
        {
            networkManager.matchMaker.DropConnection(match.networkId, match.nodeId, 0, networkManager.OnDropConnection);
            networkManager.StopHost();
        }
        RefreshRoomList();

    }


}
