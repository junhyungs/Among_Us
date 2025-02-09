using UnityEngine;
using Mirror;

public class AMONGUS_RoomPlayer : NetworkRoomPlayer
{
    public readonly SyncHashSet<NetworkRoomPlayer> _syncSet = new SyncHashSet<NetworkRoomPlayer>();

    //Hook 매개변수 -> 미러가 자동으로 채워줌. 첫 번째 매개변수는 변경 전값, 두 번째 매개변수는 변경된 값을 나타냄.
    [SyncVar(hook = nameof(PlayerColor_Hook))] //Hook -> 매개변수 타입도 일치해야함. 이전 , 변경 순서. 
    public PlayerColorType CurrentPlayerColor;
    private void PlayerColor_Hook(PlayerColorType previousColor, PlayerColorType newColor)
    {
        LobbyUIManager.Instance.CustomizeUI.UpdateColorButton();
    }

    [SyncVar]
    private string _nickName;

    public string SyncNickName
    {
        get => _nickName;
        set
        {
            if (isServer)
            {
                _nickName = value;
            }
        }
    }

    public CharacterMove CharacterMove { get; set; }
    public static AMONGUS_RoomPlayer MyPlayer { get; set; }

    //[System.Obsolete] 어트리뷰트는 메서드나 클래스가 더 이상 사용되지 않음을 알려주는 경고/오류 시스템을 제공
    //public static AMONGUS_RoomPlayer MyRoomPlayer { get; set; }

    public override void Start()
    {
        base.Start();

        if (isServer)
        {
            SpawnLobbyCharacter();

            LobbyUIManager.Instance.ActiveStartButton();
        }

        var gameRoomPlayerCount_Component = LobbyUIManager.Instance.GameRoomPlayerCount;

        gameRoomPlayerCount_Component.OnUpdatePlayerCountText();
    }

    private void OnDestroy()
    {
        if (isServer)
        {
            Debug.Log("서버");
        }

        if (isClient)
        {
            Debug.Log("클라");

            if (LobbyUIManager.Instance != null)
            {
                LobbyUIManager.Instance.CustomizeUI.OnStopClientColorButton(CurrentPlayerColor);
                //개인 인스턴스로 실행한 것 같지만 모든 클라에게 적용되는 이유.
                //이 객체의 파괴 이벤트 메서드를 실행시킨 곳은 Server임. 네트워크 객체가 파괴되면 서버는 이 정보를 모든 클라에게 <전파>하고
                //모든 클라에서 파괴된 객체의 OnDestroy를 실행하게 됨. 이건 네트워크 객체가 파괴되며 발생하는 특수한 경우임.
                //클라에서 실행하는건 어떻게 보면 당연한거 자신의 로컬에서 객체를 삭제시켜야 하기 때문.

                //서버에서 객체 파괴 -> 클라에게 파괴 정보 전달 -> 모든 클라의 OnDestroy 실행.
                //ClientRpc와 흡사함.

                var gameRoomPlayerCount_Component = LobbyUIManager.Instance.GameRoomPlayerCount;

                gameRoomPlayerCount_Component.OnUpdatePlayerCountText();
            }
        }
    }

    public void ReadyToBegin()
    {
        CmdChangeReadyState(true);
    }

    //public override void OnStartClient()
    //{
    //    if (isClient)
    //    {
    //        Debug.Log(connectionToClient.connectionId);
    //        NetworkClient.Send(new RequestAMONGUS_RoomPlayer { _user = this });
    //    }
    //}

    //public void SetPlayerColor(PlayerColorType playerColor)
    //{
    //    CommandSetPlayerColor(playerColor);
    //}

    //[Command]
    //public void CommandSetPlayerColor(PlayerColorType playerColor)
    //{
    //    CurrentPlayerColor = playerColor;

    //    if(_characterMove == null)
    //    {
    //        return;
    //    }

    //    _characterMove.ColorType = playerColor;
    //}

    private void SpawnLobbyCharacter()
    {
        var roomSlots = (NetworkManager.singleton as AMONGUS_RoomManager).roomSlots;
        //내가 착각한거. AMONGUS_RoomManager는 NetworkRoomManager를 상속받는데 왜 roomSlots이라는 public 필드에 접근할 수 없는걸까.
        //1. AMONGUS_RoomManager는 singleton class가 아니다 이는 NetworkRoomManager 역시 마찬가지. singleton을 사용하는 클래스를 상속한다고 자식 클래스까지 전역 접근이 되는건 아니니까.
        //2. AMONUS_RoomManager.singleton은 되지만, 이는 AMONGUS_RoomManager로 접근하는게 아니라 NetwrokManager로 접근하는 것. 그러니까 당연히 안되지;; 
        
        
        PlayerColorType playerColorType = PlayerColorType.Red;

        for(int i = 0; i < (int)PlayerColorType.Lime; i++)
        {
            bool isFindPlayerColor = false;

            foreach(var roomPlayer in roomSlots)
            {
                var amongusRoomplayer = roomPlayer as AMONGUS_RoomPlayer;

                if(amongusRoomplayer.CurrentPlayerColor == (PlayerColorType)i && roomPlayer.netId != netId)
                {
                    //현재 netId가 자기 자신의 netId와 다른지 확인. 자신의 슬롯은 제외하고 다른 플레이어의 슬롯에서 확인하기 위함. (현재 색상(CurrentColor)을 사용할 수 있는지 여부 검사).
                    //netId -> NetworkIdentity에서 제공하는 고유 ID. 네트워크안에서 각 객체를 구분한다.
                    isFindPlayerColor = true;

                    break;
                }
            }

            if (!isFindPlayerColor) //roomSlots을 순회하면서 결국 일치하는 색상을 발견하지 못했다면, 현재 플레이어 색상을 발견하지 못한 색상으로 변경한다.
            {
                playerColorType = (PlayerColorType)i;
                break;
            }
        }

        CurrentPlayerColor = playerColorType;

        var index = SpawnPositions.instance.Index;
        var spawnPosition = SpawnPositions.instance.GetSpawnPosition();
        
        var playerObject = Instantiate(AMONGUS_RoomManager.singleton.spawnPrefabs[0], spawnPosition, Quaternion.identity); //spawnPrefabs -> 미리 등록해 놓은 네트워크 프리팹 리스트. 

        var lobbyCharacterMoveComponent = playerObject.GetComponent<LobbyCharacterMove>();

        NetworkServer.Spawn(playerObject, connectionToClient);

        Vector3 spawnDirection = index < 5 ? new Vector3(1f, 0f, 0f) : new Vector3(-1f, 0f, 0f);
        lobbyCharacterMoveComponent.ClientRPCFlipXAndMovement(spawnDirection);
        lobbyCharacterMoveComponent.SetMyRoomPlayer();
        lobbyCharacterMoveComponent.CurrentPlayerColor = playerColorType;
    }

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

    public void RequestCommanSetPlayerColor(PlayerColorType playerColorType)
    {
        if (!isOwned) //지금 이 객체에 권한은 있지만, 권한 검사를 하지 않고 커맨드를 호출할 경우 경고 발생할 수 있음.
        {
            return;
        }

        CommandSetPlayerColor(playerColorType);
    }

    [Command]
    private void CommandSetPlayerColor(PlayerColorType playerColorType)
    {
        CurrentPlayerColor = playerColorType;
    }
}
