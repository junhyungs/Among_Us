using Mirror;
using System.Collections;
using UnityEngine;

public class AMONGUS_RoomManager : NetworkRoomManager
{ 
    public static AMONGUS_RoomManager Instance
    {
        get
        {
            if(NetworkManager.singleton != null)
            {
                var instance = NetworkManager.singleton as AMONGUS_RoomManager;

                return instance;
            }

            var roomManager = FindFirstObjectByType<AMONGUS_RoomManager>();

            if(roomManager != null)
            {
                return roomManager;
            }

            throw new System.Exception("RoomManager is Null");
        }
    }

    public override void OnRoomServerConnect(NetworkConnectionToClient conn)//서버에서 새로 접속한 클라이언트가 있을 때 동작하는 함수 
    {
        base.OnRoomServerConnect(conn);

        StartCoroutine(AddNetworkRoomPlayer(conn));
    }

    [Server]
    private IEnumerator AddNetworkRoomPlayer(NetworkConnectionToClient conn)
    {
        yield return new WaitWhile(() => conn.identity == null);

        if(conn.identity.TryGetComponent(out AMONGUS_RoomPlayer player) )
        {
            AMONGUS_User.Instance.AddRoomPlayer(player);
        }
    }

    public override void OnRoomServerDisconnect(NetworkConnectionToClient conn)
    {
        base.OnRoomServerDisconnect(conn);

        if(conn.identity.TryGetComponent(out AMONGUS_RoomPlayer player))
        {
            AMONGUS_User.Instance.RemoveRoomPlayer(player);
        }
    }
}
