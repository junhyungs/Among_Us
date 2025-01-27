using Mirror;
using UnityEngine;

public class LobbyCharacterMove : CharacterMove
{
    [SyncVar(hook = nameof(SetOwnerNetId_Hook))]
    public uint _ownerNetId;

    private void SetOwnerNetId_Hook(uint _, uint newOwnerId)
    {
        var myRoomPlayer = AMONGUS_RoomPlayer.MyRoomPlayer;

        if(myRoomPlayer.netId == newOwnerId)
        {
            myRoomPlayer._characterMove = this;
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
