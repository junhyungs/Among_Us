using UnityEngine;

public class GameRuleItem_Button : GameRuleItem
{
    protected override void Awake()
    {
        base.Awake();

        if (!AMONGUS_RoomPlayer.MyPlayer.isServer)
        {
            InactiveObject.SetActive(false);
        }
    }
}
