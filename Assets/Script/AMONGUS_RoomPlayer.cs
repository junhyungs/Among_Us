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

    //[System.Obsolete] ��Ʈ����Ʈ�� �޼��峪 Ŭ������ �� �̻� ������ ������ �˷��ִ� ���/���� �ý����� ����
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
        //���� �����Ѱ�. AMONGUS_RoomManager�� NetworkRoomManager�� ��ӹ޴µ� �� roomSlots�̶�� public �ʵ忡 ������ �� ���°ɱ�.
        //1. AMONGUS_RoomManager�� singleton class�� �ƴϴ� �̴� NetworkRoomManager ���� ��������. singleton�� ����ϴ� Ŭ������ ����Ѵٰ� �ڽ� Ŭ�������� ���� ������ �Ǵ°� �ƴϴϱ�.
        //2. AMONUS_RoomManager.singleton�� ������, �̴� AMONGUS_RoomManager�� �����ϴ°� �ƴ϶� NetwrokManager�� �����ϴ� ��. �׷��ϱ� �翬�� �ȵ���;; 

        PlayerColorType playerColorType = PlayerColorType.Red;

        for(int i = 0; i < (int)PlayerColorType.Lime; i++)
        {
            bool isFindPlayerColor = false;

            foreach(var roomPlayer in roomSlots)
            {
                var amongusRoomplayer = roomPlayer as AMONGUS_RoomPlayer;

                if(amongusRoomplayer.CurrentPlayerColor == (PlayerColorType)i && roomPlayer.netId != netId)
                {
                    //���� netId�� �ڱ� �ڽ��� netId�� �ٸ��� Ȯ��. �ڽ��� ������ �����ϰ� �ٸ� �÷��̾��� ���Կ��� Ȯ���ϱ� ����. (���� ����(CurrentColor)�� ����� �� �ִ��� ���� �˻�).
                    //netId -> NetworkIdentity���� �����ϴ� ���� ID. ��Ʈ��ũ�ȿ��� �� ��ü�� �����Ѵ�.
                    isFindPlayerColor = true;

                    break;
                }
            }

            if (!isFindPlayerColor) //roomSlots�� ��ȸ�ϸ鼭 �ᱹ ��ġ�ϴ� ������ �߰����� ���ߴٸ�, ���� �÷��̾� ������ �߰����� ���� �������� �����Ѵ�.
            {
                playerColorType = (PlayerColorType)i;
                break;
            }
        }

        CurrentPlayerColor = playerColorType;

        var spawnPosition = SpawnPositions.instance.GetSpawnPosition();

        var playerObject = Instantiate(AMONGUS_RoomManager.singleton.spawnPrefabs[0], spawnPosition, Quaternion.identity); //spawnPrefabs -> �̸� ����� ���� ��Ʈ��ũ ������ ����Ʈ. 

        var lobbyCharacterMoveComponent = playerObject.GetComponent<LobbyCharacterMove>();

        NetworkServer.Spawn(playerObject, connectionToClient);

        lobbyCharacterMoveComponent._ownerNetId = netId; //_ownerNetId ������ �ڽ��� netId�� �־������ν� CharacterMove ��ü�� �Ҵ�޵��� ��.
        lobbyCharacterMoveComponent.ColorType = playerColorType;
    }
}
