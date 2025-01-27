using UnityEngine;
using Mirror;

public class AMONGUS_RoomPlayer : NetworkRoomPlayer
{
    [SyncVar(hook = nameof(PlayerColor_Hook))]
    public PlayerColorType CurrentPlayerColor;
    private void PlayerColor_Hook(PlayerColorType previousColor, PlayerColorType newColor)
    {
        LobbyUIManager.Instance.CustomizeUI.UpdateColorButton();
    }

    public CharacterMove _characterMove;

    private static AMONGUS_RoomPlayer _myRoomPlayer;

    //[System.Obsolete] 어트리뷰트는 메서드나 클래스가 더 이상 사용되지 않음을 알려주는 경고/오류 시스템을 제공
    public static AMONGUS_RoomPlayer MyRoomPlayer
    {
        get
        {
            if(_myRoomPlayer == null)
            {
                _myRoomPlayer = GetMyRoomPlayer();
            }

            return _myRoomPlayer;
        }
    }

    private static AMONGUS_RoomPlayer GetMyRoomPlayer()
    {
        if (NetworkManager.singleton is AMONGUS_RoomManager roomManager)
        {
            var roomSlots = roomManager.roomSlots;

            foreach (var networkroomplayer in roomSlots)
            {
                if (networkroomplayer.isOwned)
                {
                    _myRoomPlayer = networkroomplayer as AMONGUS_RoomPlayer;

                    break;
                }
            }
        }

        return _myRoomPlayer;
    }


    public override void Start()
    {
        base.Start();

        if (isServer)
        {
            SpawnLobbyCharacter();
        }
    }

    [Command]
    public void CommandSetPlayerColor(PlayerColorType playerColor)
    {
        CurrentPlayerColor = playerColor;

        if(_characterMove == null)
        {
            Debug.Log("<CharacterMove is Null");
            return;
        }

        _characterMove.ColorType = playerColor;
    }

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

        var spawnPosition = SpawnPositions.instance.GetSpawnPosition();

        var playerObject = Instantiate(AMONGUS_RoomManager.singleton.spawnPrefabs[0], spawnPosition, Quaternion.identity); //spawnPrefabs -> 미리 등록해 놓은 네트워크 프리팹 리스트. 

        var lobbyCharacterMoveComponent = playerObject.GetComponent<LobbyCharacterMove>();

        NetworkServer.Spawn(playerObject, connectionToClient);

        lobbyCharacterMoveComponent._ownerNetId = netId; //_ownerNetId 변수에 자신의 netId를 넣어줌으로써 CharacterMove 객체를 할당받도록 함.
        lobbyCharacterMoveComponent.ColorType = playerColorType;
    }
}
