﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class killFeed : MonoBehaviour
{
    [SerializeField]
    GameObject killfeedItemPrefab;
    void Start()
    {
        GameManager.instance.onPlayerKilledCallback += OnKill;
    }

    public void OnKill(string player, string source)
    {
        GameObject go = (GameObject)Instantiate(killfeedItemPrefab, this.transform);
        go.GetComponent<killFeedItem>().Setup(player,source);
        go.transform.SetAsFirstSibling();

        Destroy(go, 4f);
    }
}
