using UnityEngine;
using Mirror;

public class IngameCharacterMove : CharacterMove
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();

        if (isOwned)
        {
            IsMoving = true;

            var myRoomPlayer = AMONGUS_RoomPlayer.MyPlayer;

            CommandSetPlayerCharacter(myRoomPlayer.SyncNickName, myRoomPlayer.CurrentPlayerColor);
        }
    }

    [Command]
    private void CommandSetPlayerCharacter(string nickName, PlayerColorType playerColor)
    {
        SyncPlayerNickName = nickName;
        CurrentPlayerColor = playerColor;
    }
}
