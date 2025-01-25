using UnityEngine;

public class GameRoomSettingUI : Settings
{
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
