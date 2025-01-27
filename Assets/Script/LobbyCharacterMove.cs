using Mirror;
using UnityEngine;

public class LobbyCharacterMove : CharacterMove
{
    [SyncVar(hook = nameof(SetOwnerNetId_Hook))]
    public uint _ownerNetId;

    private void SetOwnerNetId_Hook(uint _, uint newOwnerId) { }

    protected override void Start()
    {
        base.Start();

        if (isOwned)
        {
            CommandGetMyRoomPlayer();
        }
    }

    [Command]
    public void CommandGetMyRoomPlayer()
    {
        var roomManager = NetworkManager.singleton as AMONGUS_RoomManager;

        foreach(var networkRoomPlayer in roomManager.roomSlots)
        {
            if(networkRoomPlayer.netId == _ownerNetId)
            {
                AMONGUS_RoomPlayer.MyRoomPlayer = networkRoomPlayer as AMONGUS_RoomPlayer;
                AMONGUS_RoomPlayer.MyRoomPlayer._characterMove = this;
                break;
            }
        }
    }


    public void OnCompleteSpawn()
    {
        if (isOwned)
        {
            IsMoving = true;
        }
    }
}
