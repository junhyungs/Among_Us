using UnityEngine;
using Mirror;

public class AMONGUS_RoomManager : NetworkRoomManager
{
    public override void OnRoomServerConnect(NetworkConnectionToClient conn)//서버에서 새로 접속한 클라이언트가 있을 때 동작하는 함수 
    {
        base.OnRoomServerConnect(conn);

        var playerObject = Instantiate(spawnPrefabs[0]); //spawnPrefabs -> 미리 등록해 놓은 네트워크 프리팹 리스트. 

        NetworkServer.Spawn(playerObject, conn);

        //var networkIdentity = playerObject.GetComponent<NetworkIdentity>();

        //networkIdentity.RemoveClientAuthority();

        //bool isAuthority = networkIdentity.AssignClientAuthority(conn); //NetworkIdentity를 통해 권한을 직접부여.

        //NetworkServer -> 서버에서 네트워크 동작을 제어하는 역할을 함.
        //서버측에서 오브젝트 생성, 동기화, 메시지 처리등을 다룸.
        //오브젝트 동기화 = 서버에서 생성된 오브젝트를 클라이언트와 동기화함. 모든 연결된 클라이언트가 동일한 오브젝트를 인식할 수 있도록 보장.
        //클라이언트 관리 = 클라이언트 연결 및 해제 처리. 클라이언트가 서버의 특정 오브젝트를 소유(권한부여)할 수 있도록 설정.
        //데이터 전송 = 클라이언트와 데이터를 주고받기 위한 메시지를 처리.

        //NetworkServer.Spawn(게임 오브젝트. NetworkConnection) 
        //네트워크 오브젝트를 생성하고, 이를 서버와 클라이언트 간에 동기화하는 역할을 하는 함수.
        //오브젝트 등록 -> 서버에서 생성된 오브젝트를 네트워크에 등록.
        //클라이언트에 전송 -> 연결된 클라이언트에게 생성된 오브젝트의 정보를 전송해 클라이언트가 해당 오브젝트를 렌더링 할 수 있도록 함.
        //권한 설정 -> 특정 클라이언트에게 오브젝트의 소유권(권한)을 부여할 수 있음.
    }
}
