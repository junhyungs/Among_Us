using UnityEngine;

public class GameRoomSettingUI : Settings
{
    protected override void OnEnable()
    {
        base.OnEnable();
        SetPlayerMove(false);
    }

    private void OnDisable()
    {
        SetPlayerMove(true);
    }

    public void ExitRoom()
    {
        var roomManager = AMONGUS_RoomManager.singleton;

        if(roomManager.mode == Mirror.NetworkManagerMode.Host)
        {
            roomManager.StopHost();
        }
        else if(roomManager.mode == Mirror.NetworkManagerMode.ClientOnly)
        {
            roomManager.StopClient();
        }
    }
}
