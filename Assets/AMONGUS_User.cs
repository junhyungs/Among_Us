using Mirror;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class AMONGUS_User : NetworkBehaviour
{
    private static AMONGUS_User _instance;

    public static AMONGUS_User Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindFirstObjectByType<AMONGUS_User>();

                if(_instance == null)
                {
                    _instance = new GameObject("Amongus_User").AddComponent<AMONGUS_User>();
                    _instance.AddComponent<NetworkIdentity>();
                }
            }

            return _instance;
        }
    }
    
    private readonly SyncHashSet<AMONGUS_RoomPlayer> _syncHashSet = new SyncHashSet<AMONGUS_RoomPlayer>();    
    public SyncHashSet<AMONGUS_RoomPlayer> SyncHashSet => _syncHashSet;

    public void AddRoomPlayer(AMONGUS_RoomPlayer player)
    {
        if (!isServer)
        {
            return;
        }

        if (!_syncHashSet.Contains(player))
        {
            _syncHashSet.Add(player);
        }
    }

    public void RemoveRoomPlayer(AMONGUS_RoomPlayer player)
    {
        if (!isServer)
        {
            return;
        }

        if (_syncHashSet.Contains(player))
        {
            _syncHashSet.Remove(player);
        }
    }
}
