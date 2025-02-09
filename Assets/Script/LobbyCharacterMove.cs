using Mirror;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class LobbyCharacterMove : CharacterMove
{
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
            if (networkRoomPlayer.isOwned)
            {
                //connectionToClient == networkRoomPlayer.connectionToClient ������
                //��Ʈ��ũ ������ �������� ���� Ŭ���̾�Ʈ�� � networkRoomPlayer�� ����Ǿ�������
                //��Ÿ����. ��Ʈ��ũ ������ �������� �ϱ� ������ ����ȭ�� ������ �Ϸ���� �ʾҴٸ�
                //�߸��� ����� �ʷ��� �� �ִ�. 
                //�ݸ� isOwned�� �������� ��Ÿ���� ������ �� ������ ����̴�.
                var amongusRoomPlayer = networkRoomPlayer as AMONGUS_RoomPlayer;
                
                AMONGUS_RoomPlayer.MyPlayer = amongusRoomPlayer;

                amongusRoomPlayer.CharacterMove = this;

                CommandSetPlayerNickName(PlayerSettings._nickName, amongusRoomPlayer);
                //Mirror���� Ŭ�� -> ������ ��ü ���� �� �ش� ��ü�� NetID�� �����Ͽ� ���������� ������ ID�� ���� ��ü�� ã�Եȴ�.
                //������ Ŭ�󿡼� ��ü�� ������ �����ٰ� �ؼ� �������� Ŭ���� ��ü�� �ٷ�� ���� �ƴ�. 
                //�ᱹ ���� �ν��Ͻ��� ����� ������. �̸� ����ȭ �������� SyncVar�� �޴��� �ؾ���.
                //���� �� �ڵ忡���� AMONGUS_RoomPlayer�� NetworkBehaviour�� ��ӹް� �ְ�, �������� �������� 
                //����Ʈ���� ��ü�� ã�� �����߱� ������ �翬�� �������� ������ ID�� ���� ��ü�� ���� ���̰�, �ڵ尡 �����ϰ� �Ǵ°���.

                break;
            }
        }
    }

    [Command]
    private void CommandSetPlayerNickName(string nickName, AMONGUS_RoomPlayer myPlayer)
    {
        myPlayer.SyncNickName = nickName;
        SyncPlayerNickName = nickName;
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
