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
                //connectionToClient == networkRoomPlayer.connectionToClient 조건은
                //네트워크 연결을 기준으로 현재 클라이언트가 어떤 networkRoomPlayer에 연결되었는지를
                //나타낸다. 네트워크 연결을 기준으로 하기 때문에 동기화가 완전히 완료되지 않았다면
                //잘못된 결과를 초래할 수 있다. 
                //반면 isOwned는 소유권을 나타내기 때문에 더 안전한 방법이다.
                var amongusRoomPlayer = networkRoomPlayer as AMONGUS_RoomPlayer;
                
                AMONGUS_RoomPlayer.MyPlayer = amongusRoomPlayer;

                amongusRoomPlayer.CharacterMove = this;

                CommandSetPlayerNickName(PlayerSettings._nickName, amongusRoomPlayer);
                //Mirror에서 클라 -> 서버로 객체 전송 시 해당 객체의 NetID를 전송하여 서버측에서 동일한 ID를 가진 객체를 찾게된다.
                //때문에 클라에서 객체를 서버로 보낸다고 해서 서버에서 클라의 객체를 다루는 것이 아님. 
                //결국 서버 인스턴스로 명령을 수행함. 이를 동기화 받으려면 SyncVar로 받던가 해야함.
                //지금 이 코드에서는 AMONGUS_RoomPlayer는 NetworkBehaviour를 상속받고 있고, 서버에서 전달해준 
                //리스트에서 객체를 찾아 전송했기 때문에 당연히 서버에도 동일한 ID를 가진 객체가 있을 것이고, 코드가 동작하게 되는거임.

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

    //            roomSlotIdentity.AssignClientAuthority(identity.connectionToClient); //매개변수 -> 현재 권한을 부여할 클라이언트.

    //            //AMONGUS_RoomPlayer.MyRoomPlayer = amongusRoomPlayer; <-문제점. Command는 서버 인스턴스로 호출함.
    //            //이 커맨드 메서드에서 할당해주는건 서버의 AMONGUS_RoomPlayer.MyRoomPlayer에 할당해준거임.
    //            //당연히 클라에서는 null이 될 수 밖에 없음.

    //            var amongusRoomPlayer = networkRoomPlayer as AMONGUS_RoomPlayer;

    //            TargetRPCSetMyRoomPlayer(identity, amongusRoomPlayer);
    //            break;
    //        }
    //    }
    //}

    //[TargetRpc] //서버에서 지정된 작업 후 결과를 특정 클라이언트에게 전달할 때 사용.
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
