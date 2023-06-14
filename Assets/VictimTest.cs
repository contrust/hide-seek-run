using System.Collections;
using System.Collections.Generic;
using Network;
using UnityEngine;

public class VictimTest : MonoBehaviour
{
    private CustomNetworkRoomManager RM;
    void Start()
    {
        RM = FindObjectOfType<CustomNetworkRoomManager>();
    }

    public void AsVictim()
    {
        RM.PlayAsVictim();
    }
}
