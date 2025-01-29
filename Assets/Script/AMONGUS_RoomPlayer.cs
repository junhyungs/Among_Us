using UnityEngine;
using Mirror;

public class AMONGUS_RoomPlayer : NetworkRoomPlayer
{
    public readonly SyncHashSet<NetworkRoomPlayer> _syncSet = new SyncHashSet<NetworkRoomPlayer>();

    //Hook �Ű����� -> �̷��� �ڵ����� ä����. ù ��° �Ű������� ���� ����, �� ��° �Ű������� ����� ���� ��Ÿ��.
    [SyncVar(hook = nameof(PlayerColor_Hook))] //Hook -> �Ű����� Ÿ�Ե� ��ġ�ؾ���. ���� , ���� ����. 
    public PlayerColorType CurrentPlayerColor;
    private void PlayerColor_Hook(PlayerColorType previousColor, PlayerColorType newColor)
    {
        LobbyUIManager.Instance.CustomizeUI.UpdateColorButton();
    }

    public CharacterMove CharacterMove { get; set; }
    public static AMONGUS_RoomPlayer MyPlayer { get; set; }

    //[System.Obsolete] ��Ʈ����Ʈ�� �޼��峪 Ŭ������ �� �̻� ������ ������ �˷��ִ� ���/���� �ý����� ����
    //public static AMONGUS_RoomPlayer MyRoomPlayer { get; set; }

    public override void Start()
    {
        base.Start();

        if (isServer)
        {
            SpawnLobbyCharacter();
        }
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

        lobbyCharacterMoveComponent.SetMyRoomPlayer();
        lobbyCharacterMoveComponent.CurrentPlayerColor = playerColorType;
    }

    //NetworkServer -> �������� ��Ʈ��ũ ������ �����ϴ� ������ ��.
    //���������� ������Ʈ ����, ����ȭ, �޽��� ó������ �ٷ�.
    //������Ʈ ����ȭ = �������� ������ ������Ʈ�� Ŭ���̾�Ʈ�� ����ȭ��. ��� ����� Ŭ���̾�Ʈ�� ������ ������Ʈ�� �ν��� �� �ֵ��� ����.
    //Ŭ���̾�Ʈ ���� = Ŭ���̾�Ʈ ���� �� ���� ó��. Ŭ���̾�Ʈ�� ������ Ư�� ������Ʈ�� ����(���Ѻο�)�� �� �ֵ��� ����.
    //������ ���� = Ŭ���̾�Ʈ�� �����͸� �ְ�ޱ� ���� �޽����� ó��.

    //NetworkServer.Spawn(���� ������Ʈ. NetworkConnection) 
    //��Ʈ��ũ ������Ʈ�� �����ϰ�, �̸� ������ Ŭ���̾�Ʈ ���� ����ȭ�ϴ� ������ �ϴ� �Լ�.
    //������Ʈ ��� -> �������� ������ ������Ʈ�� ��Ʈ��ũ�� ���.
    //Ŭ���̾�Ʈ�� ���� -> ����� Ŭ���̾�Ʈ���� ������ ������Ʈ�� ������ ������ Ŭ���̾�Ʈ�� �ش� ������Ʈ�� ������ �� �� �ֵ��� ��.
    //���� ���� -> Ư�� Ŭ���̾�Ʈ���� ������Ʈ�� ������(����)�� �ο��� �� ����.

    public void RequestCommanSetPlayerColor(PlayerColorType playerColorType)
    {
        if (!isOwned) //���� �� ��ü�� ������ ������, ���� �˻縦 ���� �ʰ� Ŀ�ǵ带 ȣ���� ��� ��� �߻��� �� ����.
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
