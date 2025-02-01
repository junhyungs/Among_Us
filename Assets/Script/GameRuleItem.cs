using UnityEngine;

public class GameRuleItem : MonoBehaviour
{
    [SerializeField] private GameObject _inactiveObject;

    void Start()
    {
        if (!AMONGUS_RoomPlayer.MyPlayer.isServer)
        {
            _inactiveObject.SetActive(false); 
        }
    }

    
}
