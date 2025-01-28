using Mirror;
using System.Collections.Generic;
using UnityEngine;


public class AMONGUS_User : NetworkBehaviour
{
    public static AMONGUS_User Instance;

    public AMONGUS_RoomPlayer MyRoomPlayer;
    
    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(Instance);
        }

        Instance = this;
    }
}
