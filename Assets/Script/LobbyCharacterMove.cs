using Mirror;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class LobbyCharacterMove : CharacterMove
{
    //Hook �Ű����� -> �̷��� �ڵ����� ä����. ù ��° �Ű������� ���� ����, �� ��° �Ű������� ����� ���� ��Ÿ��.
    [SyncVar(hook = nameof(SetOwnerNetId_Hook))]
    public uint _ownerNetId;

    private void SetOwnerNetId_Hook(uint _, uint newOwnerId)
    {
        var roomManager = (NetworkManager.singleton as AMONGUS_RoomManager);

        foreach(var networkRoomPlayer in roomManager.roomSlots)
        {
            if(connectionToClient == networkRoomPlayer.connectionToClient)
            {
                var amongusRoomPlayer = networkRoomPlayer as AMONGUS_RoomPlayer;

                AMONGUS_User.Instance.MyRoomPlayer = amongusRoomPlayer;

                amongusRoomPlayer.CharacterMove = this;

                break;
            }
        }
    }

    [Server]
    public void SetMyRoomPlayer()
    {
        var roomManager = (NetworkManager.singleton as AMONGUS_RoomManager);

        MyRoomPlayer(roomManager.roomSlots.ToList());
    }

    [TargetRpc]
    private void MyRoomPlayer(List<NetworkRoomPlayer> roomPlayerList)
    {
        foreach (var networkRoomPlayer in roomPlayerList)
        {
            if (connectionToClient == networkRoomPlayer.connectionToClient)
            {
                var amongusRoomPlayer = networkRoomPlayer as AMONGUS_RoomPlayer;

                AMONGUS_User.Instance.MyRoomPlayer = amongusRoomPlayer;

                amongusRoomPlayer.CharacterMove = this;

                break;
            }
        }
    }


    #region CommandAndTargetRPC
    //[Command]
    //public void FindMyRoomPlayer(NetworkIdentity identity)
    //{
    //    var roomSlots = (NetworkManager.singleton as AMONGUS_RoomManager).roomSlots;

    //    foreach(var networkRoomPlayer in roomSlots)
    //    {
    //        if(networkRoomPlayer.connectionToClient == identity.connectionToClient)
    //        {
    //            var roomSlotIdentity = networkRoomPlayer.GetComponent<NetworkIdentity>();

    //            roomSlotIdentity.AssignClientAuthority(identity.connectionToClient); //�Ű����� -> ���� ������ �ο��� Ŭ���̾�Ʈ.

    //            //AMONGUS_RoomPlayer.MyRoomPlayer = amongusRoomPlayer; <-������. Command�� ���� �ν��Ͻ��� ȣ����.
    //            //�� Ŀ�ǵ� �޼��忡�� �Ҵ����ִ°� ������ AMONGUS_RoomPlayer.MyRoomPlayer�� �Ҵ����ذ���.
    //            //�翬�� Ŭ�󿡼��� null�� �� �� �ۿ� ����.

    //            var amongusRoomPlayer = networkRoomPlayer as AMONGUS_RoomPlayer;

    //            TargetRPCSetMyRoomPlayer(identity, amongusRoomPlayer);
    //            break;
    //        }
    //    }
    //}

    //[TargetRpc] //�������� ������ �۾� �� ����� Ư�� Ŭ���̾�Ʈ���� ������ �� ���.
    //public void TargetRPCSetMyRoomPlayer(NetworkIdentity identity, AMONGUS_RoomPlayer roomPlayer)
    //{
    //    if(identity.netId == netId)
    //    {
    //        AMONGUS_RoomPlayer.MyRoomPlayer = roomPlayer;

    //        if(AMONGUS_RoomPlayer.MyRoomPlayer != null)
    //        {
    //            AMONGUS_RoomPlayer.MyRoomPlayer._characterMove = this;
    //        }
    //    }
    //}
    #endregion

    public void OnCompleteSpawn()
    {
        if (isOwned)
        {
            IsMoving = true;
        }
    }
}
