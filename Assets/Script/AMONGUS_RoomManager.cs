using Mirror;
using System.Collections;
using UnityEngine;

public class AMONGUS_RoomManager : NetworkRoomManager
{
    public int MinPlayerCount;
    public int ImposterCount;

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

    public override void OnRoomServerConnect(NetworkConnectionToClient conn)//�������� ���� ������ Ŭ���̾�Ʈ�� ���� �� �����ϴ� �Լ� 
    {
        OnUpdatePlayerCount();
    }

    public override void OnRoomServerDisconnect(NetworkConnectionToClient conn)
    {
        OnUpdatePlayerCount();
    }

    [Server]
    private void OnUpdatePlayerCount()
    {
        var connectionsCount = NetworkServer.connections.Count;

        var gameRoomPlayerCount_Component = LobbyUIManager.Instance.GameRoomPlayerCount;

        gameRoomPlayerCount_Component.SyncCurrentPlayerCount = connectionsCount;
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

    

    [Server]
    private void RemoveNetworkRoomPlayer(NetworkConnectionToClient conn)
    {
        if (conn.identity.TryGetComponent(out AMONGUS_RoomPlayer player))
        {
            AMONGUS_User.Instance.RemoveRoomPlayer(player);
        }
    }

    //public override void OnServerDisconnect(NetworkConnectionToClient conn)
    //{
    //    var identity = conn.identity;

    //    if(identity != null)
    //    {
    //        AMONGUS_RoomPlayer amongusRoomPlayer = identity.GetComponent<AMONGUS_RoomPlayer>(); 

    //        NetworkServer.SendToAll()
    //    }


    //    base.OnServerDisconnect(conn);
    //}


}
